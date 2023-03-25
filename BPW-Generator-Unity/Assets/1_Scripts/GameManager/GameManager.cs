using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int currentTurn;
    //Ups
    public float PhysicsFrameRate; 
    private float physicsTimer;

    private List<BaseClassEarly> baseClassEarlyList;
    private List<BaseClass> baseClassList;
    private List<BaseClassLate> baseClassLateList;

    private void Awake()
    {
        baseClassEarlyList = new List<BaseClassEarly>();
        baseClassList = new List<BaseClass>();
        baseClassLateList = new List<BaseClassLate>();

        baseClassEarlyList.AddRange(FindObjectsOfType<BaseClassEarly>());
        baseClassList.AddRange(FindObjectsOfType<BaseClass>());
        baseClassLateList.AddRange(FindObjectsOfType<BaseClassLate>());

        for (int i = 0; i < baseClassEarlyList.Count; i++) { baseClassEarlyList[i].OnAwake(); }
        for (int i = 0; i < baseClassList.Count; i++) { baseClassList[i].OnAwake(); }
        for (int i = 0; i < baseClassLateList.Count; i++) { baseClassLateList[i].OnAwake(); }
    }

    private void Start()
    {
        for (int i = 0; i < baseClassEarlyList.Count; i++) { baseClassEarlyList[i].OnStart(); }
        for (int i = 0; i < baseClassList.Count; i++) { baseClassList[i].OnStart(); }
        for (int i = 0; i < baseClassLateList.Count; i++) { baseClassLateList[i].OnStart(); }
        EventManager.OnNextTurn += OnNextTurn;
    }

    private void Update()
    {
        //Calc PhysicsUpdate()
        if (physicsTimer > 1 / PhysicsFrameRate)
        {
            PhysicsUpdate();
            physicsTimer = 0;
        }
        physicsTimer += Time.deltaTime;

        for (int i = 0; i < baseClassEarlyList.Count; i++) { baseClassEarlyList[i].OnUpdate(); }
        for (int i = 0; i < baseClassList.Count; i++) { baseClassList[i].OnUpdate(); }
        for (int i = 0; i < baseClassLateList.Count; i++) { baseClassLateList[i].OnUpdate(); }
    }

    private void OnNextTurn()
    {
        currentTurn++;
    }

    private void PhysicsUpdate()
    {
        for (int i = 0; i < baseClassEarlyList.Count; i++) { baseClassEarlyList[i].OnPhysicsUpdate(); }
        for (int i = 0; i < baseClassList.Count; i++) { baseClassList[i].OnPhysicsUpdate(); }
        for (int i = 0; i < baseClassLateList.Count; i++) { baseClassLateList[i].OnPhysicsUpdate(); }
    }

    public void AddObject(GameObject currentObject)
    {
        BaseClass[] scripts = currentObject.GetComponents<BaseClass>();

        for (int i = 0; i < scripts.Length; i++) 
        {
            scripts[i].OnAwake();
        }

        for (int i = 0; i < scripts.Length; i++)
        {
            scripts[i].OnStart();
        }

        for (int i = 0; i < scripts.Length; i++)
        {
            baseClassList.Add(scripts[i]);
        }
    }

    public void RemoveObject(BaseClass _object)
    {
        baseClassList.Remove(_object);
        Destroy(_object.gameObject);
    }
}