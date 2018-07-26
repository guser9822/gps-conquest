using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedInterpol("{\"inter\":[0]")]
	public partial class TowerCaptureNetworkControllerNetworkObject : NetworkObject
	{
		public const int IDENTITY = 10;

		private byte[] _dirtyFields = new byte[1];

		#pragma warning disable 0067
		public event FieldChangedEvent fieldAltered;
		#pragma warning restore 0067
		private uint _TowerEntityNetId;
		public event FieldEvent<uint> TowerEntityNetIdChanged;
		public Interpolated<uint> TowerEntityNetIdInterpolation = new Interpolated<uint>() { LerpT = 0f, Enabled = false };
		public uint TowerEntityNetId
		{
			get { return _TowerEntityNetId; }
			set
			{
				// Don't do anything if the value is the same
				if (_TowerEntityNetId == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x1;
				_TowerEntityNetId = value;
				hasDirtyFields = true;
			}
		}

		public void SetTowerEntityNetIdDirty()
		{
			_dirtyFields[0] |= 0x1;
			hasDirtyFields = true;
		}

		private void RunChange_TowerEntityNetId(ulong timestep)
		{
			if (TowerEntityNetIdChanged != null) TowerEntityNetIdChanged(_TowerEntityNetId, timestep);
			if (fieldAltered != null) fieldAltered("TowerEntityNetId", _TowerEntityNetId, timestep);
		}

		protected override void OwnershipChanged()
		{
			base.OwnershipChanged();
			SnapInterpolations();
		}
		
		public void SnapInterpolations()
		{
			TowerEntityNetIdInterpolation.current = TowerEntityNetIdInterpolation.target;
		}

		public override int UniqueIdentity { get { return IDENTITY; } }

		protected override BMSByte WritePayload(BMSByte data)
		{
			UnityObjectMapper.Instance.MapBytes(data, _TowerEntityNetId);

			return data;
		}

		protected override void ReadPayload(BMSByte payload, ulong timestep)
		{
			_TowerEntityNetId = UnityObjectMapper.Instance.Map<uint>(payload);
			TowerEntityNetIdInterpolation.current = _TowerEntityNetId;
			TowerEntityNetIdInterpolation.target = _TowerEntityNetId;
			RunChange_TowerEntityNetId(timestep);
		}

		protected override BMSByte SerializeDirtyFields()
		{
			dirtyFieldsData.Clear();
			dirtyFieldsData.Append(_dirtyFields);

			if ((0x1 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _TowerEntityNetId);

			// Reset all the dirty fields
			for (int i = 0; i < _dirtyFields.Length; i++)
				_dirtyFields[i] = 0;

			return dirtyFieldsData;
		}

		protected override void ReadDirtyFields(BMSByte data, ulong timestep)
		{
			if (readDirtyFlags == null)
				Initialize();

			Buffer.BlockCopy(data.byteArr, data.StartIndex(), readDirtyFlags, 0, readDirtyFlags.Length);
			data.MoveStartIndex(readDirtyFlags.Length);

			if ((0x1 & readDirtyFlags[0]) != 0)
			{
				if (TowerEntityNetIdInterpolation.Enabled)
				{
					TowerEntityNetIdInterpolation.target = UnityObjectMapper.Instance.Map<uint>(data);
					TowerEntityNetIdInterpolation.Timestep = timestep;
				}
				else
				{
					_TowerEntityNetId = UnityObjectMapper.Instance.Map<uint>(data);
					RunChange_TowerEntityNetId(timestep);
				}
			}
		}

		public override void InterpolateUpdate()
		{
			if (IsOwner)
				return;

			if (TowerEntityNetIdInterpolation.Enabled && !TowerEntityNetIdInterpolation.current.UnityNear(TowerEntityNetIdInterpolation.target, 0.0015f))
			{
				_TowerEntityNetId = (uint)TowerEntityNetIdInterpolation.Interpolate();
				//RunChange_TowerEntityNetId(TowerEntityNetIdInterpolation.Timestep);
			}
		}

		private void Initialize()
		{
			if (readDirtyFlags == null)
				readDirtyFlags = new byte[1];

		}

		public TowerCaptureNetworkControllerNetworkObject() : base() { Initialize(); }
		public TowerCaptureNetworkControllerNetworkObject(NetWorker networker, INetworkBehavior networkBehavior = null, int createCode = 0, byte[] metadata = null) : base(networker, networkBehavior, createCode, metadata) { Initialize(); }
		public TowerCaptureNetworkControllerNetworkObject(NetWorker networker, uint serverId, FrameStream frame) : base(networker, serverId, frame) { Initialize(); }

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}
