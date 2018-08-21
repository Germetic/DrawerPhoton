using UnityEngine;

public class PlayerController : Photon.MonoBehaviour {

    //Transform freezes optimization
    private Vector3 _oldBrushPos = Vector3.zero;
    private Vector3 _newBrushPos = Vector3.zero;
    private float _offsetTime = 0f;
    private bool _isWasFirstSynch = false;

    private void Start()
    {
        photonView.RPC("SetRandomColor",PhotonTargets.All);
        photonView.RPC("ChangePlayersCount",PhotonTargets.AllBuffered);
    }
    [PunRPC]
    private void ChangePlayersCount()
    {
        JoinToMatchController.Instance.CountOfPlayers.text = "PLAYERS : " + PhotonNetwork.playerList.Length;

    }
    private void OnPhotonSerializeView(PhotonStream stream,PhotonMessageInfo info)
    {
        Vector3 pos = transform.position;
        stream.Serialize(ref pos);
        if (stream.isReading)
        {
            _oldBrushPos = transform.position;
            _newBrushPos = pos;
            _offsetTime = 0;
            _isWasFirstSynch = true;
          
        }
    }
    private void Update()
    {   
 
        if (photonView.isMine)
        {
            if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0))
            {
                photonView.RPC("СlearLineOnAllUsers", PhotonTargets.All);
            }
            if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetMouseButton(0))
            {
                Plane tempPlanePoint = new Plane(Camera.main.transform.forward * -1, this.transform.position);
                Ray mouseClickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                float rayDistance;
                if (tempPlanePoint.Raycast(mouseClickRay, out rayDistance))
                {
                    this.transform.position = mouseClickRay.GetPoint(rayDistance);
                }
            }

        }
        else if (_isWasFirstSynch)
        {
            if (Vector3.Distance(_oldBrushPos, _newBrushPos) > 2f) transform.position = _oldBrushPos = _newBrushPos;
            else
            {
                _offsetTime += Time.deltaTime * 9f;
                transform.position = Vector3.Lerp(_oldBrushPos,_newBrushPos,_offsetTime);
            }
        }
    }

    [PunRPC]
    private void СlearLineOnAllUsers()
    {
        GetComponent<TrailRenderer>().Clear();
    }

    [PunRPC]
    public void SetRandomColor()
    {
        Gradient gradient = new Gradient();
        gradient.SetKeys
            (
            new GradientColorKey[] { new GradientColorKey(GetRandomColor(), 0f)},
            new GradientAlphaKey[] { new GradientAlphaKey(255f, 0f) }
            );
       
        GetComponent<TrailRenderer>().colorGradient = gradient;
    }

    private Color GetRandomColor()
    {
        int index =  UnityEngine.Random.Range(0,5);
        Color color = Color.white;
        switch (index)
        {
            case 0:
                color = Color.red;
                break;
            case 1:
                color = Color.green;
                break;
            case 2:
                color = Color.blue;
                break;
            case 3:
                color = Color.yellow;
                break;
            case 4:
                color = Color.magenta;
                break;
            case 5:
                color = Color.white;
                break;
        }
        return color;
    }
    private void OnDestroy()
    {
        photonView.RPC("ChangePlayersCount", PhotonTargets.AllBuffered);
    }
}
