using BeardedManStudios.Forge.Networking.Frame;
using System;
using MainThreadManager = BeardedManStudios.Forge.Networking.Unity.MainThreadManager;

namespace BeardedManStudios.Forge.Networking.Generated
{
	public partial class NetworkObjectFactory : NetworkObjectFactoryBase
	{
		public override void NetworkCreateObject(NetWorker networker, int identity, uint id, FrameStream frame, Action<NetworkObject> callback)
		{
			if (networker.IsServer)
			{
				if (frame.Sender != null && frame.Sender != networker.Me)
				{
					if (!ValidateCreateRequest(networker, identity, id, frame))
						return;
				}
			}
			
			bool availableCallback = false;
			NetworkObject obj = null;
			MainThreadManager.Run(() =>
			{
				switch (identity)
				{
					case ChatManagerNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new ChatManagerNetworkObject(networker, id, frame);
						break;
					case CubeForgeGameNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new CubeForgeGameNetworkObject(networker, id, frame);
						break;
					case ExampleProximityPlayerNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new ExampleProximityPlayerNetworkObject(networker, id, frame);
						break;
					case GameFactionsNetworkControllerNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new GameFactionsNetworkControllerNetworkObject(networker, id, frame);
						break;
					case GameUINetworkEntityNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new GameUINetworkEntityNetworkObject(networker, id, frame);
						break;
					case NetworkCameraNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new NetworkCameraNetworkObject(networker, id, frame);
						break;
					case PlayerAvatorControllerNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new PlayerAvatorControllerNetworkObject(networker, id, frame);
						break;
					case PlayerDestinationControllerNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new PlayerDestinationControllerNetworkObject(networker, id, frame);
						break;
					case PlayerEntityModelNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new PlayerEntityModelNetworkObject(networker, id, frame);
						break;
					case TestNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new TestNetworkObject(networker, id, frame);
						break;
					case TowerCaptureNetworkControllerNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new TowerCaptureNetworkControllerNetworkObject(networker, id, frame);
						break;
					case TowerEntityControllerNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new TowerEntityControllerNetworkObject(networker, id, frame);
						break;
					case TowerUIEntityNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new TowerUIEntityNetworkObject(networker, id, frame);
						break;
				}

				if (!availableCallback)
					base.NetworkCreateObject(networker, identity, id, frame, callback);
				else if (callback != null)
					callback(obj);
			});
		}

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}