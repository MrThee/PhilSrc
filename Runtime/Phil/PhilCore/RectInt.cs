using UnityEngine;

namespace Phil.Core {


[System.Serializable]
public struct RectInt : System.IEquatable<RectInt> {

	public int2 position;
	public int2 dimensions;
	public int width { get { return dimensions.x; } }
	public int height { get { return dimensions.y; } }
	public int area { get { return width * height; } }
	public int top{ get { return position.y + height - 1; } }
	public int bottom{ get { return position.y; } }
	public int left{ get { return position.x; } }
	public int right{ get { return position.x + width - 1; } }

	public RectInt topEdge { get { return new RectInt (this.left, this.top, this.width, 1); } }
	public RectInt botEdge { get { return new RectInt (this.left, this.bottom, this.width, 1); } }
	public RectInt leftEdge { get { return new RectInt (this.left, this.bottom, 1, this.height); } }
	public RectInt rightEdge { get { return new RectInt (this.right, this.bottom, 1, this.height); } }

	public RectInt(int2 position, int2 size){
		this.position = position;
		this.dimensions = size;
	}

	public RectInt(int xPosition, int yPosition, int width, int height){
		this.position = new int2 (xPosition, yPosition);
		this.dimensions = new int2 (width, height);
	}

	public RectInt(Transform transform){
		this.position = new int2 (Mathf.RoundToInt (transform.position.x), Mathf.RoundToInt (transform.position.z));
		this.dimensions = new int2 (Mathf.RoundToInt (transform.lossyScale.x), Mathf.RoundToInt (transform.lossyScale.z));
	}

	public RectInt(Rect rect){
		int flooredX = Mathf.FloorToInt (0.5f + rect.xMin);
		int flooredY = Mathf.FloorToInt (0.5f + rect.yMin);
		int ceiledX = Mathf.CeilToInt (0.5f + rect.xMax);
		int ceiledY = Mathf.CeilToInt (0.5f + rect.yMax);

		this.position = new int2 (flooredX, flooredY);
		this.dimensions = new int2 (ceiledX - flooredX, ceiledY - flooredY);
	}

	public int2 middle { get { return new int2( (right+left)/2, (bottom + top)/2 ); } }
	public Vector2 center { get { return new Vector2 ((right + left) / 2.0f, (bottom + top) / 2.0f); } }
	public Vector3 worldCenter { 
		get {
			return new Vector3 ((right + left) / 2.0f, 0f, (bottom + top) / 2.0f); 
		} 
	}

	public int2 north { get { return new int2 (middle.x, top); } }
	public int2 south { get { return new int2 (middle.x, bottom); } }
	public int2 east { get { return new int2 (right, middle.y); } }
	public int2 west { get { return new int2 (left, middle.y); } }

	public int2 northWest { get { return new int2 (left, top); } }
	public int2 southWest { get { return new int2 (left, bottom); } }
	public int2 northEast { get { return new int2 (right, top); } }
	public int2 southEast { get { return new int2 (right, bottom); } }

	public Rect WorldRect(){
		
		Vector2 pos = new Vector2 ((float)position.x - 0.5f, (float)position.y - 0.5f);
		Vector2 dims = new Vector2 (width, height);

		return new Rect (pos, dims);
	}

	public bool WithinWorldRect(Vector3 position){
		Rect rect = WorldRect ();
		return (rect.xMin < position.x && position.x < rect.xMax &&
			rect.yMin < position.z && position.z < rect.yMax);
		// We're exclusive because edges are negligible (for now OpieOP)
	}
	public bool Contains(int2 coord){
		if (this.left <= coord.x && this.bottom <= coord.y && coord.x <= this.right && coord.y <= this.top) {
			return true;
		} else {
			return false;
		}
	}
	public bool IsOnEdge(int2 coord){
		if (this.left == coord.x || this.right == coord.x || this.top == coord.y || this.bottom == coord.y) {
			return true;
		} else {
			return false;
		}
	}

	public int2 Clamp(int2 input){
		return new int2(
			Mathf.Clamp(input.x, this.southWest.x, this.northEast.x),
			Mathf.Clamp(input.y, this.southWest.y, this.northEast.y)
		);
	}

	public bool Overlaps(RectInt other){

		return (
			this.bottom <= other.top 
			&& this.top >= other.bottom
			&& this.left <= other.right 
			&& this.right >= other.left);

	}

	public int2 GetCoord(int flatIndex){

		flatIndex = Mathf.Clamp (flatIndex, 0, area - 1);

		int xCoord = position.x + (flatIndex % width);
		int yCoord = position.y + (flatIndex / width);

		return new int2 (xCoord, yCoord);
	}

	public bool IsSubsetOf(RectInt largerDomain){
		return ((this & largerDomain).area > 0);
	}

	public static bool operator ==(RectInt a, RectInt b){
		return (a.dimensions == b.dimensions && a.position == b.position);
	}

	public static bool operator !=(RectInt a, RectInt b){
		return !(a == b);
	}

	public static RectInt operator &(RectInt a, RectInt b){
		if (!a.Overlaps (b)) {
			return new RectInt(0, 0, 0, 0);
		} else {
			int maxLeft = Mathf.Max (a.left, b.left);
			int minRight = Mathf.Min (a.right, b.right);
			int minTop = Mathf.Min (a.top, b.top);
			int maxBot = Mathf.Max (a.bottom, b.bottom);
			int width = minRight - maxLeft + 1;
			int height = minTop - maxBot + 1;
			return new RectInt ( new int2 (maxLeft, maxBot), new int2 (width, height) );
		}
	}

	public override bool Equals (object obj)
	{
		return base.Equals (obj);
	}
	
	public bool Equals(RectInt other){
		return this == other;
	}

	public override int GetHashCode ()
	{
		return base.GetHashCode ();
	}

	public override string ToString ()
	{
		return string.Format ("[RectInt: width={0}, height={1}, area={2}, top={3}, bottom={4}, left={5}, right={6}, middle={7}, center={8}, worldCenter={9}]", width, height, area, top, bottom, left, right, middle, center, worldCenter);
	}

}


}