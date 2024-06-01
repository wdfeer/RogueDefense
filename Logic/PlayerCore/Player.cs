using Godot;
using System.Collections.Generic;
using System.Linq;
using RogueDefense.Logic.Enemies;
using RogueDefense.Logic.PlayerProjectile;

namespace RogueDefense.Logic.PlayerCore;

public partial class Player
{
    public static Player my;
    public bool Local => id == Client.myId;
    public static Dictionary<int, Player> players;
    public int id;
    public string Name => Local ? SaveData.name : Client.instance.GetUserData(id).name;

    public List<PlayerHooks> hooks;
    public ShootManager shootManager;
    public UpgradeManager upgradeManager;
    public AbilityManager abilityManager;
    public int[] augmentPoints;
    public Player(int id, int[] upgradePointDistribution)
    {
        augmentPoints = upgradePointDistribution;

        InitializeHooks();

        this.id = id;
        players.Add(id, this);
        shootManager = new ShootManager(this);
        upgradeManager = new UpgradeManager(this);
        abilityManager = new AbilityManager(this);
        SpawnTurret();

        if (Local) hooks.Add(new HpResetter(this));
    }
    void InitializeHooks()
        => hooks = new List<PlayerHooks>()
        {
            new DpsCounterPlayer(this), new StatusPlayer(this), new FirstShotPlayer(this), new FirstHitPlayer(this),
            new NthShotMultishotPlayer(this), new MaxHpPerKillPlayer(this), new DamagePerUniqueStatusPlayer(this),
            new LowEnemyHpDamagePlayer(this), new MultishotPerShotPlayer(this), new DamageVsArmorPlayer(this),
            new ExplosionPlayer(this), new RecoveryPlayer(this), new CritChanceOnStunnedPlayer(this)
        };
    public void _Process(double delta)
    {
        hooks.ForEach(x => x.PreUpdate((float)delta));
        upgradeManager.Process((float)delta);
        hooks.ForEach(x => x.PostUpgradeUpdate((float)delta));
        shootManager.Process((float)delta);
        hooks.ForEach(x => x.PostUpdate((float)delta));

        if (target == null || !GodotObject.IsInstanceValid(target) || target.Dead)
            FindTarget();
    }
    public void _PhysicsProcess(double delta)
    {
        if (this == my)
            UpdateMovement(delta);
    }
    void UpdateMovement(double delta)
    {
        if (controlledTurret.Stunned)
            return;

        Vector2 inputDirection = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        controlledTurret.Velocity = inputDirection * Turret.SPEED * 1000 * (float)delta;
        controlledTurret.MoveAndSlide();

        if (NetworkManager.Singleplayer || inputDirection == Vector2.Zero)
            return;
        Vector2 pos = controlledTurret.GlobalPosition;
        SendPositionUpdateMessage(Client.myId, turrets.FindIndex(x => x == controlledTurret), pos.X, pos.Y);
    }
    static void SendPositionUpdateMessage(int client, int turretIndex, float x, float y)
    {
        Client.instance.SendMessage(MessageType.PositionUpdated, new string[] { client.ToString(), turretIndex.ToString(), x.ToString(), y.ToString() });
    }
    public List<Turret> turrets = new List<Turret>();
    public IEnumerable<Turret> ActiveTurrets => turrets.Where(x => !x.Stunned);
    public Turret controlledTurret;
    public Enemy target;
    bool IsValidTarget(Enemy enemy)
        => enemy != null && GodotObject.IsInstanceValid(enemy) && enemy.Targetable;
    void FindTarget()
    {
        for (int i = 0; i < Enemy.enemies.Count; i++)
        {
            Enemy enemy = Enemy.enemies[i];
            if (IsValidTarget(enemy))
            {
                SetTarget(i);
                return;
            }
        }
    }
    public void SetTarget(int enemyIndex, bool netUpdate = true)
    {
        if (enemyIndex >= Enemy.enemies.Count)
            return;

        Enemy enemy = Enemy.enemies[enemyIndex];

        if (!IsValidTarget(enemy))
            return;

        target = enemy;
        foreach (Turret turret in turrets)
        {
            turret.target = enemy;
        }

        if (netUpdate && Client.client != null)
        {
            Client.instance.SendMessage(MessageType.TargetSelected, new string[] { Client.myId.ToString(), enemyIndex.ToString() });
        }
    }
    public void SpawnTurret()
    {
        controlledTurret = DefenseObjective.instance.turretScene.Instantiate<Turret>();
        controlledTurret.owner = this;
        DefenseObjective.instance.AddChild(controlledTurret);
        controlledTurret.Position += new Vector2(-50f + GD.Randf() * 200f, (GD.Randf() - 0.5f) * 300);
        turrets.Add(controlledTurret);

        controlledTurret.SetLabel(string.Concat(Name.Take(3)).ToUpper());

        controlledTurret.target = target;
    }

    public void OnWaveEnd()
    {
        shootManager.ClearBullets(x => x is FusedBullet);
        hooks.ForEach(x => x.OnWaveEnd());
        upgradeManager.UpdateUpgrades();
        upgradeManager.UpdateUpgradeText();
        shootManager.shootCount = 0;
    }
}
