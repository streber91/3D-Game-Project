﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Underlord.Animation
{
    public class Bone
    {
        #region Fields

        private Bone parent = null;
        private List<Bone> children = new List<Bone>();

        private Matrix bindTransform = Matrix.Identity;
        private Vector3 bindScale = Vector3.One;
        private Vector3 translation = Vector3.Zero;
        private Quaternion rotation = Quaternion.Identity;
        private Vector3 scale = Vector3.One;

        #endregion

        #region Properties

        public string Name = "";

        public Matrix BindTransform { get { return bindTransform; } }
        public Matrix SkinTransform { get; set; }
        public Quaternion Rotation { get { return rotation; } set { rotation = value; } }
        public Vector3 Translation { get { return translation; } set { translation = value; } }
        public Vector3 Scale { get { return scale; } set { scale = value; } }
        public Bone Parent { get { return parent; } }
        public List<Bone> Children { get { return children; } }
        public Matrix AbsoluteTransform = Matrix.Identity;

        #endregion

        #region Constuctor

        public Bone(string name, Matrix bindTransform, Bone parent)
        {
            this.Name = name;
            this.parent = parent;
            if (parent != null)
                parent.children.Add(this);

            this.bindScale = new Vector3(bindTransform.Right.Length(),
                bindTransform.Up.Length(), bindTransform.Backward.Length());

            bindTransform.Right = bindTransform.Right / bindScale.X;
            bindTransform.Up = bindTransform.Up / bindScale.Y;
            bindTransform.Backward = bindTransform.Backward / bindScale.Y;
            this.bindTransform = bindTransform;

            ComputeAbsoluteTransform();
            SkinTransform = Matrix.Invert(AbsoluteTransform);
        }
        #endregion

        #region Transform

        public void ComputeAbsoluteTransform()
        {
            Matrix transform = Matrix.CreateScale(Scale * bindScale) *
                Matrix.CreateFromQuaternion(Rotation) *
                Matrix.CreateTranslation(Translation) *
                BindTransform;

            if (Parent != null)
            {
                AbsoluteTransform = transform * Parent.AbsoluteTransform;
            }
            else
            {  
                AbsoluteTransform = transform;
            }
        }

        public void SetCompleteTransform(Matrix m)
        {
            Matrix setTo = m * Matrix.Invert(BindTransform);

            Translation = setTo.Translation;
            Rotation = Quaternion.CreateFromRotationMatrix(setTo);
        }

        #endregion

    }
}
