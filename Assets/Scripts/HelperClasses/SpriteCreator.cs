using UnityEngine;
using System.Collections;
using System.IO;

namespace DefaultNamespace
{
    public static class SpriteCreator
    {
        public static Sprite LoadNewSprite(string filePath, float pixelsPerUnit = 100.0f, SpriteMeshType spriteMeshType = SpriteMeshType.FullRect)
        {
            var spriteTexture = LoadTexture(filePath);
            var newSprite = Sprite.Create(spriteTexture, new Rect(0, 0, spriteTexture.width, spriteTexture.height),
                new Vector2(0, 0), pixelsPerUnit, 0, spriteMeshType);

            return newSprite;
        }

        public static Sprite ConvertTextureToSprite(Texture2D texture, float pixelsPerUnit = 100.0f, SpriteMeshType spriteMeshType = SpriteMeshType.FullRect)
        {
            var newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0),
                pixelsPerUnit, 0, spriteMeshType);

            return newSprite;
        }

        private static Texture2D LoadTexture(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Debug.LogError("file path returned null");
                return null;
            }
            
            var fileData = File.ReadAllBytes(filePath);
            var tex2d = new Texture2D(2,2);

            if (tex2d.LoadImage(fileData)) 
                return tex2d;
            
            Debug.LogError("Texture returned null");
            return null;

        }
    }
}