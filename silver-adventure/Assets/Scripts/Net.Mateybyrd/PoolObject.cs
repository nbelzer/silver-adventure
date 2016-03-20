using UnityEngine;
using System.Collections;

namespace Net.Mateybyrd {
  public class PoolObject : MonoBehaviour {

    public virtual void OnObjectReuse() {

    }

    protected void Destroy() {
      gameObject.SetActive (false);
    }
  }
}