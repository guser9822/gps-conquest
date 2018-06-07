using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedRPC("{\"types\":[[][\"uint\", \"bool\", \"string\"][\"string\", \"uint\", \"double\"]]")]
	[GeneratedRPCVariableNames("{\"types\":[[][\"_playerNetId\", \"_isCapturing\", \"_playerNickname\"][\"_playerUsername\", \"_playerNetId\", \"_playerCaptureTimePassed\"]]")]
	public abstract partial class TowerCaptureNetworkControllerBehavior : NetworkBehavior
	{
		public const byte RPC_UPDATE_CAPTURE_CONTROLLER_ON_NETWORK = 0 + 5;
		public const byte RPC_UPDATE_CAPTURE_ON_NETWORK = 1 + 5;
		public const byte RPC_UPDATE_CAPTURE_TIME_FOR_PLAYER = 2 + 5;
		
		public TowerCaptureNetworkControllerNetworkObject networkObject = null;

		public override void Initialize(NetworkObject obj)
		{
			// We have already initialized this object
			if (networkObject != null && networkObject.AttachedBehavior != null)
				return;
			
			networkObject = (TowerCaptureNetworkControllerNetworkObject)obj;
			networkObject.AttachedBehavior = this;

			base.SetupHelperRpcs(networkObject);
			networkObject.RegisterRpc("UpdateCaptureControllerOnNetwork", UpdateCaptureControllerOnNetwork);
			networkObject.RegisterRpc("UpdateCaptureOnNetwork", UpdateCaptureOnNetwork, typeof(uint), typeof(bool), typeof(string));
			networkObject.RegisterRpc("UpdateCaptureTimeForPlayer", UpdateCaptureTimeForPlayer, typeof(string), typeof(uint), typeof(double));

			networkObject.onDestroy += DestroyGameObject;

			if (!obj.IsOwner)
			{
				if (!skipAttachIds.ContainsKey(obj.NetworkId))
					ProcessOthers(gameObject.transform, obj.NetworkId + 1);
				else
					skipAttachIds.Remove(obj.NetworkId);
			}

			if (obj.Metadata != null)
			{
				byte transformFlags = obj.Metadata[0];

				if (transformFlags != 0)
				{
					BMSByte metadataTransform = new BMSByte();
					metadataTransform.Clone(obj.Metadata);
					metadataTransform.MoveStartIndex(1);

					if ((transformFlags & 0x01) != 0 && (transformFlags & 0x02) != 0)
					{
						MainThreadManager.Run(() =>
						{
							transform.position = ObjectMapper.Instance.Map<Vector3>(metadataTransform);
							transform.rotation = ObjectMapper.Instance.Map<Quaternion>(metadataTransform);
						});
					}
					else if ((transformFlags & 0x01) != 0)
					{
						MainThreadManager.Run(() => { transform.position = ObjectMapper.Instance.Map<Vector3>(metadataTransform); });
					}
					else if ((transformFlags & 0x02) != 0)
					{
						MainThreadManager.Run(() => { transform.rotation = ObjectMapper.Instance.Map<Quaternion>(metadataTransform); });
					}
				}
			}

			MainThreadManager.Run(() =>
			{
				NetworkStart();
				networkObject.Networker.FlushCreateActions(networkObject);
			});
		}

		protected override void CompleteRegistration()
		{
			base.CompleteRegistration();
			networkObject.ReleaseCreateBuffer();
		}

		public override void Initialize(NetWorker networker, byte[] metadata = null)
		{
			Initialize(new TowerCaptureNetworkControllerNetworkObject(networker, createCode: TempAttachCode, metadata: metadata));
		}

		private void DestroyGameObject(NetWorker sender)
		{
			MainThreadManager.Run(() => { try { Destroy(gameObject); } catch { } });
			networkObject.onDestroy -= DestroyGameObject;
		}

		public override NetworkObject CreateNetworkObject(NetWorker networker, int createCode, byte[] metadata = null)
		{
			return new TowerCaptureNetworkControllerNetworkObject(networker, this, createCode, metadata);
		}

		protected override void InitializedTransform()
		{
			networkObject.SnapInterpolations();
		}

		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void UpdateCaptureControllerOnNetwork(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void UpdateCaptureOnNetwork(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void UpdateCaptureTimeForPlayer(RpcArgs args);

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}