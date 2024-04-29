using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class MapSpriteSelector : MonoBehaviour
{
    public Sprite   spU, spD, spR,spL, spUD, spRL, spUR, spUL, spDR, spDL, spULD, spRUL, spDRU, spLDR, spUDRL; //rooms sprites
    public bool up, down, left, right;
    public int type; // 0: normal, :enter
    public Color normalColor, enterColor, endColor;
    Color mainColor;
    SpriteRenderer rend;

    void Start () {
        rend = GetComponent<SpriteRenderer>();
        mainColor = normalColor;
        PickSprite();
        PickColor();
    }

    void PickSprite(){
        if (up){
            if (down){
                if (right){
                    if (left){
                        rend.sprite = spUDRL;                    
                    }else{
                        rend.sprite = spDRU;
                    }                    
                }else if (left){
                    rend.sprite = spULD;
                }else{
                    rend.sprite = spUD;
                }
            }else{
                if (right){
                    if (left){
                        rend.sprite = spRUL;
                    }else{
                        rend.sprite = spUR;
                    }
                }else if (left){
                    rend.sprite = spUL;
                }else{
                    rend.sprite = spU;
                }
            }
            return;
        }
        if (down){
            if (right){
                if(left){
                    rend.sprite = spLDR;
                }else{
                    rend.sprite = spDR;
                }
            }else if (left){
                rend.sprite = spDL;
            }else{
                rend.sprite = spD;
            }
            return;
        }
        if (right){
            if (left){
                rend.sprite = spRL;                
            }else{
                rend.sprite = spR;
            }
        }else{
            rend.sprite = spL;
        }
    }

    void PickColor(){
        if (type == 0){
            mainColor = normalColor;
            Debug.Log("Picking normal color for room at position " + transform.position);
        }else if (type == 1){
            mainColor = enterColor;
            Debug.Log("Picking enter color for room at position " + transform.position);
        }else if (type == 2){
            mainColor = endColor;
            Debug.Log("Picking end color for room at position " + transform.position);
        }
        rend.color = mainColor;
    }

}
