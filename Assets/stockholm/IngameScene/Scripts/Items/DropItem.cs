using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 게임상에 드롭된 아이템 클래스입니다.
/// 이 클래스는 Item을 반드시 자식으로 가지고 있습니다.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class DropItem : MonoBehaviour
{
	public Item item;
	public HitboxArea hitbox;

	private SpriteRenderer renderer_;
	private Vector3 originPos_;

	void Awake()
	{
		renderer_ = this.GetComponent<SpriteRenderer>();
		renderer_.color = new Color(1, 1, 1, 0);
	}

	/// <summary>
	/// 플레이어가 Hitbox에 들어왔을 때 실행되는 함수입니다.
	/// </summary>
	/// <param name="target"></param>
	private void EnterPlayer(GameObject target)
	{
		if (item != null)
		{
			target.GetComponentInChildren<Backpack>().AddConsumeItem((ConsumeItem)item);
			GiveItemAndDestroy(target.transform);
		}
	}

	/// <summary>
	/// Dropitem이 자신이 드랍할 아이템을 가집니다.
	/// </summary>
	/// <param name="item"></param>
	public void GetItem(Item item)
	{
		item.gameObject.transform.parent = this.transform;
		renderer_.sprite = item.itemImageSmall;

		this.item = item;
	}

	/// <summary>
	/// DropItem을 보여줍니다.
	/// 몬스터가 죽으면 이 함수를 실행해 보여주면 됩니다.
	/// </summary>
	public void ShowDropItem(Vector3 pos)
	{
		// TODO: 나올 때 아이템 회전하게
		this.transform.position = pos;
		renderer_.DOFade(1, 0.3f).OnComplete(()=> hitbox.EnterGameObjectEvent += EnterPlayer);
		this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 5f);
	}

	/// <summary>
	/// DropItem이 가지고 있는 Item을 반환하고 자신은 스스로 파괴합니다.
	/// 파괴되면서 targetPos를 따라갑니다.
	/// </summary>
	/// <param name="targetPos">파괴되면서 따라갈 transform입니다.</param>
	/// <returns>DropItem이 가지고 있는 Item입니다.</returns>
	public Item GiveItemAndDestroy(Transform targetPos)
	{
		Item temp = null;
		if (item != null)
		{
			temp = item;
			item = null;

			originPos_ = this.transform.position;
			StartCoroutine(MovePlayer(targetPos));
		}
		return temp;
	}
	/// <summary>
	/// 플레이어를 따라가고 스스로 파괴되는 코루틴입니다.
	/// </summary>
	/// <param name="target"></param>
	private IEnumerator MovePlayer(Transform target)
	{
		float t = 0f;
		while(this.transform.position != target.position) {
			float lerpX = Mathf.Lerp(originPos_.x, target.position.x, t);
			float lerpY = Mathf.Lerp(originPos_.y, target.position.y, t);
			float lerpZ = Mathf.Lerp(originPos_.z, target.position.z, t);

			this.transform.position = new Vector3(lerpX, lerpY, lerpZ);
			renderer_.color = new Color(1, 1, 1, Mathf.Lerp(1f, 0f, t));

			t += 3 * Time.deltaTime;

			yield return null;
		}
		Destroy(this.gameObject);
	}
}
