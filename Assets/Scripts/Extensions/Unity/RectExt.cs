using UnityEngine;

namespace Extensions.Unity
{
    public static class RectExt
    {
        public static Vector2 GetSceneViewMousePos(this Rect camPixelRect, Vector2 current)
        {
            Vector2 camTopLeftPxPos = new Vector2
            (camPixelRect.position.x, camPixelRect.position.y + camPixelRect.height);

            Vector2 normalizedInverseMouse = current - camTopLeftPxPos;

            Vector2 inSceneMousePos = new Vector2
            (
                normalizedInverseMouse.x,
                (camPixelRect.height - (normalizedInverseMouse.y + camPixelRect.height))
            );

            return inSceneMousePos;
        }
    }
}