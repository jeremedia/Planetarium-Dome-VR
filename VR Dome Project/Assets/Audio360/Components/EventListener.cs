/*
 *  Copyright 2013-Present Two Big Ears Limited
 *  All rights reserved.
 *  http://twobigears.com
 */

using UnityEngine;
using System.Collections;

namespace TBE
{
    public delegate void EventDelegate(TBE.Event e);
    
    public class EventListener : NativeEventListener
    {
        public EventDelegate newEvent;

        protected override void onNewEvent(TBE.Event e)
        {
            if (newEvent != null)
            {
                newEvent(e);
            }
        }
    }

}