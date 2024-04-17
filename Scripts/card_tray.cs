using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class card_tray : MonoBehaviour,IDropHandler
{
    public int index_tray;
    public Transform area_tray;
    private List<GameObject> list_carde = new List<GameObject>();
    public bool is_tray_done = false;
    public bool is_get_group = false;
    public bool is_tray_docking = false;
    public Card_Type card_type = Card_Type.None;
    public Animator anim;
    public Color32 color_reset;

    public void add_card(GameObject obj_card)
    {
        obj_card.transform.SetParent(this.area_tray);
        obj_card.transform.position = new Vector3(obj_card.transform.position.x, obj_card.transform.position.y,0f);
        obj_card.transform.localScale = new Vector3(1f, 1f, 1f);
        int index_old_tray = obj_card.GetComponent<card_obj>().index_tray;
        if (index_old_tray != -1)
        {
            GameObject.Find("Game").GetComponent<Game_handle>().manager_card.remove_carde_in_tray(index_old_tray);
        }
        obj_card.GetComponent<card_obj>().index_tray = index_tray;
        this.list_carde.Add(obj_card);
    }

    public GameObject get_card_random()
    {
        if (this.list_carde.Count == 0) return null;
        int index_obj = Random.Range(0, this.list_carde.Count);
        GameObject obj_move=Instantiate(this.list_carde[index_obj]);
        Destroy(this.list_carde[index_obj]);
        this.list_carde.RemoveAt(index_obj);
        return obj_move;
    }

    public GameObject get_card_last_and_reamove()
    {
        int index_obj = this.list_carde.Count-1;
        GameObject obj_move = Instantiate(this.list_carde[index_obj]);
        Destroy(this.list_carde[index_obj]);
        this.list_carde.RemoveAt(index_obj);
        return obj_move;
    }

    public GameObject get_card_last()
    {
        if (this.list_carde.Count == 0) return null;
        int index_obj = this.list_carde.Count - 1;
        return this.list_carde[index_obj];
    }

    public void remove_card_last()
    {
        if (this.list_carde.Count == 0) return;

        int index_obj_last = this.list_carde.Count - 1;
        this.list_carde.RemoveAt(index_obj_last);
        this.open_card_last();
    }

    private void open_card_last()
    {
        if (this.list_carde.Count == 0) return;
        int index_obj_last = this.list_carde.Count - 1;
        this.list_carde[index_obj_last].GetComponent<card_obj>().open();
    }

    public void close_in_tray_no_first()
    {
        for(int i = 0; i < this.list_carde.Count; i++) this.list_carde[i].GetComponent<card_obj>().close();

        int index_last_card = this.list_carde.Count - 1;
        this.list_carde[index_last_card].GetComponent<card_obj>().open();
    }

    public void unclick_all_card()
    {
        for (int i = 0; i < this.list_carde.Count; i++) this.list_carde[i].GetComponent<card_obj>().unclick();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            GameObject obj_card_drag = eventData.pointerDrag.gameObject;
            card_obj obj_card_inst = obj_card_drag.GetComponent<card_obj>();
            if (obj_card_inst.get_status_open())
            {
                
                if (this.is_tray_done)
                {
                    if ((obj_card_inst.card_type != this.card_type) || (obj_card_inst.card_number != this.list_carde.Count + 1))
                    {
                        GameObject.Find("Game").GetComponent<Game_handle>().play_sound(1);
                        return;
                    }
                }
                else
                {
                    if (this.list_carde.Count<=0)
                    {
                        if (obj_card_inst.card_number != 13)
                        {
                            GameObject.Find("Game").GetComponent<Game_handle>().play_sound(1);
                            return;
                        }
                        else
                        {
                            this.add_card_to_tray_true(obj_card_drag);
                            return;
                        }
                    }

                    card_obj obj_card_last = this.get_card_last().GetComponent<card_obj>();


                    if (obj_card_last.card_type == Card_Type.Spades)
                    {
                        if ((obj_card_inst.card_type == Card_Type.Spades) ||(obj_card_inst.card_type == Card_Type.Clubs)||(obj_card_inst.card_number!=obj_card_last.card_number-1))
                        {
                            GameObject.Find("Game").GetComponent<Game_handle>().play_sound(1);
                            return;
                        }

                    }else if (obj_card_last.card_type == Card_Type.Clubs)
                    {
                        if ((obj_card_inst.card_type == Card_Type.Clubs) || (obj_card_inst.card_type == Card_Type.Spades) || (obj_card_inst.card_number != obj_card_last.card_number - 1))
                        {
                            GameObject.Find("Game").GetComponent<Game_handle>().play_sound(1);
                            return;
                        }
                    }
                    else if(obj_card_last.card_type == Card_Type.Hearts)
                    {
                        if ((obj_card_inst.card_type == Card_Type.Hearts) || (obj_card_inst.card_type == Card_Type.Diamond) || (obj_card_inst.card_number != obj_card_last.card_number - 1))
                        {
                            GameObject.Find("Game").GetComponent<Game_handle>().play_sound(1);
                            return;
                        }
                    }
                    else if (obj_card_last.card_type == Card_Type.Diamond)
                    {
                        if ((obj_card_inst.card_type == Card_Type.Diamond) || (obj_card_inst.card_type == Card_Type.Hearts) || (obj_card_inst.card_number != obj_card_last.card_number - 1))
                        {
                            GameObject.Find("Game").GetComponent<Game_handle>().play_sound(1);
                            return;
                        }
                    }
                }

                this.add_card_to_tray_true(obj_card_drag);
                if (this.is_tray_done) GameObject.Find("Game").GetComponent<Game_handle>().manager_card.check_win_game();
            }
        }
    }

    private void add_card_to_tray_true(GameObject obj_card_drag)
    {
        obj_card_drag.GetComponent<card_obj>().set_transform_pater(this.transform);
        this.add_card(obj_card_drag);
        List<GameObject> list_goup_card=obj_card_drag.GetComponent<card_obj>().get_list_goup_card();

        if (list_goup_card.Count > 0)
        {
            for(int i=0;i<list_goup_card.Count;i++) this.add_card(list_goup_card[i]);
        }

        obj_card_drag.transform.position = this.transform.position;
        if (this.is_tray_done) obj_card_drag.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        GameObject.Find("Game").GetComponent<Game_handle>().play_sound(2);
        GameObject.Find("Game").GetComponent<Game_handle>().manager_card.add_count_move();
    }

    public int get_count_card()
    {
        return this.list_carde.Count;
    }

    public void check_group(card_obj card_check)
    {
        if (this.is_get_group)
        {
            for (int i = 0; i < this.list_carde.Count; i++)
            {
                if ((this.list_carde[i].GetComponent<card_obj>().card_number < card_check.card_number) && this.list_carde[i].GetComponent<card_obj>().get_status_open())
                {
                    this.list_carde[i].transform.SetParent(card_check.transform);
                    card_check.add_to_group(this.list_carde[i]);
                }
            }
        }
    }

    public void clear()
    {
        for(int i = 0; i < this.list_carde.Count; i++)
        {
            Destroy(this.list_carde[i].gameObject);
        }
        this.list_carde = new List<GameObject>();
    }

    public void check_true_in_tray(card_obj card_check)
    {
        if(this.is_tray_docking==true)
        {
            if (this.get_card_last() != null)
            {
                card_obj card_last = this.get_card_last().GetComponent<card_obj>();

                if (card_last != null)
                {
                    if(card_last.card_number == card_check.card_number + 1)
                    {
                        if (card_last.card_type == Card_Type.Spades)
                        {
                            if ((card_check.card_type == Card_Type.Spades) || (card_check.card_type == Card_Type.Clubs)) return;
                        }
                        else if (card_last.card_type == Card_Type.Clubs)
                        {
                            if ((card_check.card_type == Card_Type.Clubs) || (card_check.card_type == Card_Type.Spades)) return; 
                        }
                        else if (card_last.card_type == Card_Type.Hearts)
                        {
                            if ((card_check.card_type == Card_Type.Hearts) || (card_check.card_type == Card_Type.Diamond)) return;
                        }
                        else if (card_last.card_type == Card_Type.Diamond)
                        {
                            if ((card_check.card_type == Card_Type.Diamond) || (card_check.card_type == Card_Type.Hearts)) return;
                        }
                        this.anim.enabled = true;
                    }
                }
            }

            if (this.list_carde.Count == 0 && card_check.card_number == 13)
            {
                this.anim.enabled = true;
            }
        }

        if (this.is_tray_done)
        {
            if (card_check.card_type == this.card_type && card_check.card_number == this.list_carde.Count + 1&&card_check.get_list_goup_card().Count==0) this.anim.enabled = true;
        }
    }

    public void reset_check()
    {
        this.anim.enabled = false;
        this.GetComponent<Image>().color = this.color_reset;
    }
}
