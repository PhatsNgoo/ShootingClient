using ShootingNetwork;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

public class NetworkManager : MonoBehaviour
{
	public List<VisitorEmail> visitorEmailList;
	private void Start()
	{
		AcceptHttpsRequest();
		// string appFirst="{"+'"'+"emailList"+'"'+":";
		string visitorEmailResponse=MessageManager.GetResponseContent(GetVisitorEmail());
		Debug.Log(visitorEmailResponse);
		visitorEmailList=JsonConvert.DeserializeObject<List<VisitorEmail>>(visitorEmailResponse);
	}
	public bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
	{
		return true;
	}
	public HttpWebResponse TestGenerateRequest()
	{
		HttpWebRequest newReq = MessageManager.GenerateRequest(URLManager.LocalUrl, "", NetworkMethod.Get, "", "", false);
		return MessageManager.GetResponse(newReq);
	}
	public HttpWebResponse GetVisitorEmail(){
		HttpWebRequest newReq=MessageManager.GenerateRequest(URLManager.LocalUrl+APIManager.GetUserEmail,"",NetworkMethod.Get,"","",false);
		return MessageManager.GetResponse(newReq);
	}
	void AcceptHttpsRequest(){
		ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
	}
}

