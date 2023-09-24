using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueDefense.Logic
{
    public partial class MovingKinematicBody2D : CharacterBody2D
    {
        /// <summary>
        /// Position change per second
        /// </summary>
        public Godot.Vector2 velocity = Godot.Vector2.Zero;
        public override void _PhysicsProcess(float delta)
        {
            var collision = MoveAndCollide(velocity);
            if (collision != null)
            {
                OnCollision(collision);
            }
        }

        protected virtual void OnCollision(KinematicCollision2D collision) { }
    }
}
