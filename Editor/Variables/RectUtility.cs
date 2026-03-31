using UnityEngine;

namespace Thimble.Editor
{
    public static class RectUtility
    {
        public static RectOffset One => new RectOffset(1, 1, 1, 1);

        public static RectOffset Multiply(this RectOffset rectOffset, float multiplier)
        {
            return new RectOffset(
                Mathf.RoundToInt(rectOffset.left * multiplier),
                Mathf.RoundToInt(rectOffset.right * multiplier),
                Mathf.RoundToInt(rectOffset.top * multiplier),
                Mathf.RoundToInt(rectOffset.bottom * multiplier)
            );
        }
    }
}
