﻿using Common.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.types.serverNotifications
{
    class NewGameState : ServerNotification
    {
        public GameState NewState { get; set; }
    }
}