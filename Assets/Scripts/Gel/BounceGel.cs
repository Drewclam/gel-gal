﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceGel : Gel {
    protected override void OnCollisionEnter2DHook(Collision2D collision) {
        ContactPoint2D contact = collision.GetContact(0);
        SpawnArea(contact.point.x, contact.point.y, contact.normal);
    }
}
