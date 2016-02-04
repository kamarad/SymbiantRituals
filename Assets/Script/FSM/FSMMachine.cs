using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FSMMachine<T> : FSMState where T: FSMState {

  protected T defaultState;
  protected T currentState;
  protected T goalState;
  protected List<T> states = new List<T>();
  protected string name;
  protected int goalId;

  public FSMMachine(string name) : base() {
    this.name = name;
  }

  public void updateMachine(float delta) {
    if (states.Count == 0) {
      return;
    }

    if (currentState == null) {
      currentState = defaultState;
      if (currentState != null) {
        currentState.enter();
      }
    }
    if (currentState == null) {
      return;
    }

    int oldState = currentState.getType();
    goalId = currentState.checkTransitions();
    //Debug.Log(name + " old: " + oldState + " goal: " + goalId);
    if (goalId != oldState) {
      if (transistionState(goalId)) {
        //Debug.Log("switching state for " + name);
        currentState.exit();
        currentState = goalState;
        currentState.enter();
      }
    }
    currentState.update(delta);
    //Debug.Log(name + " cur: " + currentState.getType());
  }

  public void addState(T s) {
    states.Add(s);
  }

  bool transistionState(int goalId) {
    if (states.Count == 0) {
      return false;
    }
    for (int i = 0; i < states.Count; i++) {
      if (states[i].getType() == goalId) {
        goalState = states[i];
        return true;
      }
    }
    return false;
  }

  public void reset() {
    exit();

    if (currentState != null) {
      currentState.exit();
    }
    currentState = defaultState;

    for (int i = 0; i < states.Count; i++) {
      states[i].init();
    }

    if (currentState != null) {
      currentState.enter();
    }
  }

  public override int checkTransitions() {
    return 0;
  }

  public override void enter() {
  }

  public override void exit() {
    if (currentState != null) {
      currentState.exit();
    }
  }

  public override void init() {
  }

  public override void update(float delta) {
  }

  public void setDefaultState(T s) {
    this.defaultState = s;
  }

  public T getCurrentState() {
    return currentState;
  }
}
