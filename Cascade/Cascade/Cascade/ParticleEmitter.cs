﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Cascade
{
    public delegate void ParticleEmittedEventHandler(ParticleEmittedEventArgs e);
    public class ParticleEmittedEventArgs : EventArgs
    {
        public ParticleEmitter Emitter;
        public Particle Particle;
    }
    public class ParticleEmitter
    {
        public Vector3 Pos = Vector3.Zero;
        public Vector3 PosRange = Vector3.Zero;
        public float Step = 1;
        float timer = 0;
        protected ParticleManager manager;
        public Vector2 Scale = Vector2.One;
        public Vector2 ScaleRange = Vector2.Zero;
        public Vector3 Speed = Vector3.Zero;
        public Vector3 SpeedRange = Vector3.Zero;
        public Color Color = Color.Gray;
        public Color ColorRange = new Color(0, 0, 0, 0);
        public event ParticleEmittedEventHandler Emitted;
        public ParticleEmitter(ParticleManager man, Vector3 pos)
        {
            Pos = pos;
            manager = man;
            man.Add(this);
        }
        public void Update()
        {
            timer += Global.Speed;
            while (timer > Step)
            {
                timer -= Step;
                var p = CreateParticle();
                p.Pos = Pos.RandomVectorRange(PosRange);
                p.Scale = Scale + new Vector2(MyMath.RandomRange(-ScaleRange.X, ScaleRange.X), MyMath.RandomRange(-ScaleRange.Y, ScaleRange.Y));
                p.Speed = Speed.RandomVectorRange(SpeedRange);
                p.Color = new Color(Color.ToVector4().RandomVectorRange(ColorRange.ToVector4()));
                if (Emitted != null)
                {
                    Emitted(new ParticleEmittedEventArgs() { Particle = p, Emitter = this });
                }
            }
        }
        protected virtual Particle CreateParticle()
        {
            return new Particle(manager, Vector3.Zero);
        }
    }
    public class CircleEmitter : ParticleEmitter
    {
        public CircleEmitter(ParticleManager man, Vector3 p)
            : base(man, p)
        {

        }
        protected override Particle CreateParticle()
        {
            return new Ellipse(manager, Vector3.Zero, 12);
        }
    }
    public class TriangleEmitter : ParticleEmitter
    {
        public TriangleEmitter(ParticleManager m, Vector3 p)
            : base(m, p)
        {

        }
        protected override Particle CreateParticle()
        {
            return new Triangle(manager, Vector3.Zero);
        }
    }
}
