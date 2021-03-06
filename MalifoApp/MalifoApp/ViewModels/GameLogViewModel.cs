﻿using Common.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;

namespace MalifoApp.ViewModels
{
    public class GameLogViewModel : ViewModel<GameLog>
    {
        private bool showSensitive;

        public GameLogViewModel(GameLog model, bool showSensitive)
            : base(model)
        {
            this.showSensitive = showSensitive;
        }

        public FlowDocument Document
        {
            get
            {
                FlowDocument document = new FlowDocument();
                document.Background = new SolidColorBrush(Color.FromArgb(0xff, 0xF9, 0xF7, 0xC2));

                // guard against null and return empty document
                if (Model == null) return document;

                foreach (GameLogEvent gameEvent in Model.Events)
                {
                    // do not show events that only the fatemaster should see
                    if (!showSensitive && gameEvent.IsSensitive)
                        continue;

                    Paragraph p = new Paragraph();
                    p.FontSize = 10;
                    
                    Run run = new Run(gameEvent.Timestamp.ToString("[HH:mm]") + " ");
                    run.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0xB0, 0x83, 0x0A));
                    p.Inlines.Add(run);

                    run = new Run(gameEvent.Playername + " ");
                    run.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x9B, 0x0A, 0x0A));
                    p.Inlines.Add(run);

                    run = new Run(gameEvent.Text);
                    p.Inlines.Add(run);

                    document.Blocks.Add(p);

                }

                return document;
            }
        }

        public void Add(GameLogEvent gameEvent)
        {
            if (Model == null)
            {
                Model = new GameLog(new List<GameLogEvent>());
            }
            Model.Events.Add(gameEvent);
            OnPropertyChanged("Document");
        }
    }
}
