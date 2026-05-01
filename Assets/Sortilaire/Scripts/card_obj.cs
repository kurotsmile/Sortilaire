using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class card_obj : MonoBehaviour,IDragHandler,IEndDragHandler,IBeginDragHandler
{
    public RectTransform rectTransform;
    public Image img_card;
    public GameObject obj_card_close;
    public int type;
    public int index_tray=-1;
    public Card_Type card_type;
    public int card_number;
    public CanvasGroup canvas_group;

    public bool is_open = false;
    private bool is_select = false;
    private Transform transform_pater;
    public Transform transform_play;
    private List<GameObject> list_card_group = new List<GameObject>();

    public void close()
    {
        this.obj_card_close.SetActive(true);
        this.is_open = false;
    }

    public void open()
    {
        this.obj_card_close.SetActive(false);
        this.is_open = true;
    }

    public void click()
    {
        if (this.is_open)
        {
            if (this.is_select == false)
            {
                GameObject.Find("Game").GetComponent<Game_handle>().manager_card.unclick_all_card();
                this.act_click();
            }
            else
            {
                this.unclick();
            }
        }
    }

    private void act_click()
    {
        if (this.is_open)
        {
            GameObject.Find("Game").GetComponent<Game_handle>().manager_card.check_in_tray_true(this);
            this.img_card.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            this.img_card.transform.rotation = Quaternion.Euler(0f, 0f, -6.82f);
            this.is_select = true;
        }
    }

    public void unclick()
    {
        this.img_card.transform.localScale = new Vector3(1f, 1f, 1f);
        this.img_card.transform.rotation = Quaternion.identity;
        this.is_select = false;
        GameObject.Find("Game").GetComponent<Game_handle>().manager_card.un_check_all_in_tray();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (this.is_open)
        {
            Vector3 pos_mouse= Camera.main.ScreenToWorldPoint(Input.mousePosition);
            this.transform.SetParent(transform_play);
            this.transform.position = new Vector3(pos_mouse.x, pos_mouse.y, 0f);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (this.is_open)
        {
            this.unclick();
            this.canvas_group.blocksRaycasts = true;
            this.transform.SetParent(this.transform_pater);
            this.transform.localPosition = new Vector3(0f, 0f, 0f);
            if (this.list_card_group.Count > 0)
            {
                for(int i = 0; i < this.list_card_group.Count; i++)
                {
                    this.list_card_group[i].transform.SetParent(this.transform_pater);
                    this.list_card_group[i].transform.localPosition = new Vector3(0f, 0f, 0f);
                }
            }
        }

        this.is_select = false;
        GameObject.Find("Game").GetComponent<Game_handle>().manager_card.un_check_all_in_tray();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (this.is_open)
        {
            this.act_click();
            this.canvas_group.blocksRaycasts = false;
            this.transform_pater = this.transform.parent;
            this.transform.SetParent(this.transform.root);
            this.transform.SetAsLastSibling();
            this.list_card_group = new List<GameObject>();
            GameObject.Find("Game").GetComponent<Game_handle>().manager_card.get_group_card(this);
        }
    }

    public bool get_status_open()
    {
        return this.is_open;
    }

    public void set_transform_pater(Transform tr)
    {
        this.transform_pater = tr;
    }

    public void add_to_group(GameObject obj_group)
    {
        this.list_card_group.Add(obj_group);
    }

    public List<GameObject> get_list_goup_card()
    {
        return this.list_card_group;
    }
}
