using UnityEngine;

namespace Phil {

public static class Phil2DExtensions {

    public static Rect GetWorldRect(this BoxCollider2D boxCollider){
        var bounds = boxCollider.bounds;
        return new Rect(bounds.min.x, bounds.min.y, bounds.size.x, bounds.size.y);
    }

}

}