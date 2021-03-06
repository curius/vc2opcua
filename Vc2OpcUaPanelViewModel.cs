﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Diagnostics;
using Caliburn.Micro;
using System.ComponentModel.Composition;
using VisualComponents.Create3D;
using VisualComponents.UX.Shared;

namespace vc2opcua
{
    [Export(typeof(IDockableScreen))]
    class Vc2OpcUaPanelViewModel : DockableScreen
    {
        VcUtils _vcutils = new VcUtils();

        Server _server;
        Thread _serverthread;

        public Vc2OpcUaPanelViewModel()
        {
            this.DisplayName = "VC2OPCUA Panel";
            this.IsPinned = true;
            this.PaneLocation = DesiredPaneLocation.DockedRight;
        }

        #region Properties

        public Collection<ISimComponent> Components { get; set; } = new Collection<ISimComponent>();

        public string Text1 { get; set; } = "";
        public string Text2 { get; set; } = "";
        public string Text3 { get; set; } = "";

        #endregion

        #region Methods

        // Add methods for listener to execute when component is added or removed
        protected override void OnActivate()
        {

        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);

        }

        public void Start()
        {
            // Start Server
            _server = new Server(false, 0);
            _serverthread = new Thread(new ThreadStart(_server.Run));

            _serverthread.Start();
            CanStart = false;
            CanStop = true;

            _vcutils.VcWriteWarningMsg("OPCUA server started");
        }

        public void Stop()
        {
            _server.Stop();
            _serverthread.Abort();

            CanStart = true;
            CanStop = false;

            _vcutils.VcWriteWarningMsg("OPCUA server stopped");
        }

        private bool _canstart = true;
        public bool CanStart
        {
            get { return _canstart; }
            private set
            {
                _canstart = value;
                NotifyOfPropertyChange(() => CanStart);
            }
        }

        private bool _canstop = false;
        public bool CanStop
        {
            get { return _canstop; }
            private set
            {
                _canstop = value;
                NotifyOfPropertyChange(() => CanStop);
            }
        }
        #endregion

    }
}
