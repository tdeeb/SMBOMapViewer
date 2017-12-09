using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SMBOMapViewer
{
    /// <summary>
    /// The camera.
    /// <para>This is a Singleton.</para>
    /// </summary>
    public class Camera
    {
        #region Singleton Fields

        public static Camera Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Camera();
                }

                return instance;
            }
        }

        private static Camera instance = null;

        #endregion

        /// <summary>
        /// The constant for translating the camera to the center of the screen
        /// </summary>
        private const float TranslationConstant = 0f;

        /// <summary>
        /// The position of the camera. (0,0) is the center of the screen
        /// </summary>
        public Vector2 Position { get; protected set; } = Vector2.Zero;

        /// <summary>
        /// The scale of the Camera. Negative values will flip everything on-screen
        /// </summary>
        public float Scale { get; protected set; } = 1f;

        /// <summary>
        /// The rotation of the camera
        /// </summary>
        public float Rotation { get; protected set; } = 0f;

        public Matrix Transform { get; protected set; } = default(Matrix);

        private Camera()
        {
            Initialize(new Vector2(0f, 0f), 0f, 1f);
        }

        public void Initialize(Vector2 position, float rotation, float scale)
        {
            SetTranslation(position);
            SetRotation(rotation);
            SetZoom(scale);
        }

        #region Transform Manipulations

        public void SetTranslation(Vector2 translation)
        {
            Position = translation;
        }

        public void Translate(Vector2 amount)
        {
            Position += amount;
        }

        public void SetRotation(float rotation)
        {
            Rotation = rotation;
        }

        public void Rotate(float amount)
        {
            Rotation += amount;
        }

        public void SetZoom(float scale)
        {
            Scale = scale;
        }

        public void Zoom(float amount)
        {
            Scale += amount;
        }

        #endregion

        /// <summary>
        /// Calculates the Camera's Transform using Matrix multiplication
        /// </summary>
        /// <returns>The Matrix representing the Camera's Transform</returns>
        public Matrix CalculateTransformation()
        {
            Vector2 windowSize = SpriteRenderer.Instance.WindowSize;
            Transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0f)) *
                        Matrix.CreateRotationZ(Rotation) *
                        Matrix.CreateScale(new Vector3(Scale, Scale, 0)) *
                        Matrix.CreateTranslation(windowSize.X * TranslationConstant, windowSize.Y * TranslationConstant, 0f);

            return Transform;
        }
    }
}
