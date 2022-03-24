using Photon.Pun;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;


    private void Start() => PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
}
