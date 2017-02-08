using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using ProgressBar;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class physics: NetworkBehaviour
{ 
	private Rigidbody2D rigidBod;

	//public static float translation = 3*Time.deltaTime;

	public int totalMass=1;

	public static Player joueurCDM ;
	public GameObject goCDM;


	public List<GameObject> playerObjects;
	public bool isRegularCDM=true;
	private Vector2 forwardCDMposition;
	
	private Vector2 rigidCDMpos;


	private GameObject go;

	public static List<Player> players;
	private Vector2 posThe;
	private int numOfPlayers;
	public static int playerLimit=2;

	void Start()
    {
    }
	// Update is called once per frame

	void Update ()
    {
		playerObjects=findPlayers();


		if(goCDM==null|| joueurCDM == null){

			goCDM = GameObject.Find("SphereCDM");
			joueurCDM=new Player(goCDM.name,/*goCDM.GetComponent<Rigidbody2D>(),*/0,goCDM,goCDM.transform.position,new ArrayList{},Color.red);
			goCDM.transform.position=cdmPosition(players,totalMass);
			players = playersArrayConstruction(playerObjects);	
		}
		if(playerObjects.Count == playerLimit)
		{
			players=playersArrayConstruction(playerObjects);

			joueurCDM.updatebufferPosition(joueurCDM);

			foreach (var player in players)
				player.updatebufferPosition(player);
			
			totalMass= sumMass(players);

			Rigidbody2D rb;
			rb=joueurCDM.go.GetComponent<Rigidbody2D>();
			//rb.MovePosition(cdmPosition(playerArr,totalMass));
			forwardCDMposition=cdmPosition(players, totalMass);
		
			//rigidCDMpos=((playerArr[0].masse*(Vector2)playerArr[0].go.transform.position) + (playerArr[1].masse*(Vector2)playerArr[1].go.transform.position) )/totalMass;

			foreach (var player in players)
				rigidCDMpos += (Vector2)player.go.transform.position*(player.mass*1.0f)/(float)totalMass; 


		if(levelDesign.levelValue!=3)
			{
				rb.MovePosition(forwardCDMposition);

			}else{

			if(levelDesign.switchLevel3==false) 
			{
			if(forwardCDMposition==rigidCDMpos)
			{
				rb.MovePosition(forwardCDMposition);
				//float alpha = Mathf.Atan(playerArr[0].go.transform.position.y/playerArr[0].go.transform.position.x);
				//playerArr[1].go.GetComponent<Rigidbody2D>().MovePosition(new Vector2(Vector3.Magnitude(playerArr[0].go.transform.position)*Mathf.Cos(alpha+Mathf.PI),Vector3.Magnitude(playerArr[0].go.transform.position)*Mathf.Sin(alpha+Mathf.PI)));
			}else{
					//float alpha = Mathf.Atan(playerArr[0].go.transform.position.y/playerArr[0].go.transform.position.x);
					//playerArr[1].go.GetComponent<Rigidbody2D>().MovePosition(new Vector2(Vector3.Magnitude(playerArr[0].go.transform.position)*Mathf.Cos(alpha+Mathf.PI),Vector3.Magnitude(playerArr[0].go.transform.position)*Mathf.Sin(alpha+Mathf.PI)));
					//playerArr[1].go.GetComponent<Rigidbody2D>().MovePosition((Vector2)joueurCDM.go.transform.position+ new Vector2(joueurCDM.go.transform.position.x- playerArr[0].go.transform.position.x, joueurCDM.go.transform.position.y- playerArr[0].go.transform.position.y));

					rb.MovePosition(forwardCDMposition);
					Debug.Log("not possible");
			}
		}
		} 
		}
	}

	public List<GameObject> findPlayers()
	{  	
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

		var res = new List<GameObject> ();
	
		foreach (var player in players)
		{
			
			if (player.name != "Sphere(Clone)" && player.name != "Sphere")
				res.Add (player);
		}
			
		return res;
	}

	public List<Player> playersArrayConstruction(List<GameObject> objs)
	{
		var res = new List<Player> ();

		foreach (var obj in objs)
		{

			//GameObject go;
			//go = GameObject.Find(arr[i].name);

			res.Add(new Player(obj.name,
				/*arr[i].GetComponent<Rigidbody2D>(),*/
				1,
				obj,
				(Vector2)obj.transform.position,
				new ArrayList{},
				Color.red)
			);
		//	Debug.Log(tabPlayer[i].namePlayer);

		}
		return res;
	}


	public int sumMass(List<Player> players)
	{
		var sum = 0;
		foreach (var player in players)
		{
			sum+= player.mass;
		}
		return sum;
	}

	public Vector2 cdmPosition(List<Player> players, int sum)
	{
		float somme = 1.0f*sum;
		Vector2 posTemp = Vector2.zero;

		Vector2 posCDM = joueurCDM.go.transform.position;
		if (players != null && players.Count >= 2)
		{
			foreach (var player in players)
				posTemp += (Vector2)player.go.transform.position * (player.mass * 1.0f) / (float)somme; 
			posCDM = posTemp;
		}
		else if (players != null && players.Count == 1)
			posCDM = (Vector2)players[0].go.transform.position;
		else
			posCDM = new Vector2(0,0);
		return posCDM;
	}

} 
