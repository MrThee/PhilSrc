using UnityEngine;

namespace Phil {

public static class Phil2DExtensions {

    // NOTE: the old implementation of this used Collider.bounds
    // which, as it turns out, doesn't update after a sync transforms.
    public static Rect GetWorldRect(this BoxCollider2D boxCollider){
        Vector2 center = boxCollider.transform.position.XY();
        Vector2 offset = boxCollider.offset;
        Vector2 lossyScale = boxCollider.transform.lossyScale.XY();

        Vector2 min = center + Vector2.Scale(lossyScale, (offset - 0.5f*boxCollider.size));
        Vector2 size = Vector2.Scale(lossyScale, boxCollider.size);
        return new Rect(min, size);
    }

}

}