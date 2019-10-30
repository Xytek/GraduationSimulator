using UnityEngine;
using System.Collections;

public interface ILookAtHandler
{
	void OnLookatEnter();
	void OnLookatExit();
	void OnLookatInteraction(Vector3 lookAtPosition, Vector3 lookAtDirection);
}
