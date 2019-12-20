using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScopeBob : MonoBehaviour {

    public float frequency;
    public float radiusOfShake;
    public float speed;
    public float dampingOfRadius;
    public float dampingOfSpeed;
	public float staminaCost = 5f;
    public bool isZoomed = false;
    Vector3 origin;
    Vector3 target;
    public bool holdingBreath = false;

	StaminaScript stamina;
    float clock;
    bool move = false;

	void Start(){
		stamina = GetComponent<StaminaScript> ();
	}

	// Update is called once per frame
	void Update () {

        if (isZoomed == false)
        {
            transform.localPosition = new Vector3(0, 0, 0);
            origin = transform.localPosition;
        }

        if (isZoomed == true && Time.time > clock && move == false)
        {
            clock = Time.time + frequency;
            Vector3 bobPosition = Random.insideUnitCircle * radiusOfShake;
            target = new Vector3(origin.x + bobPosition.x, origin.y + bobPosition.y, origin.z);
            move = true;

            if (holdingBreath == true && radiusOfShake > 0)
            {
                radiusOfShake -= Time.deltaTime * dampingOfRadius;
            }
            else if (holdingBreath == true && radiusOfShake != 0)
            {
                radiusOfShake = 0;
            }
            if (holdingBreath == true && speed > 0)
            {
                speed -= Time.deltaTime * dampingOfSpeed;
            }
            else if (holdingBreath == true && speed != 0)
            {
                speed = 0;
            }
        }

        else if (Time.time > clock &&move == true)
        {
            move = false;
        }
        
        if (move == true)
        {
            float step = speed * Time.deltaTime;
            transform.localPosition = Vector3.Lerp(transform.localPosition, target, step / 100);
        }
        if (Input.GetKeyDown(KeyCode.Z) && holdingBreath == false)
        {
            holdingBreath = true;
			stamina.UseStamina (staminaCost);
        }
        else if(Input.GetKeyDown(KeyCode.Z) && holdingBreath == true)
        {
            holdingBreath = false;
			//stamina.StoppedUsingStamina();
        }

    }
}
