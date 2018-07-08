using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedInterpol("{\"inter\":[0]")]
	public partial class TowerUIEntityNetworkObject : NetworkObject
	{
		public const int IDENTITY = 12;

		private byte[] _dirtyFields = new byte[1];

		#pragma warning disable 0067
		public event FieldChangedEvent fieldAltered;
		#pragma warning restore 0067
		private uint _TowerEntityNetID;
		public event FieldEvent<uint> TowerEntityNetIDChanged;
		public Interpolated<uint> TowerEntityNetIDInterpolation = new Interpolated<uint>() { LerpT = 0f, Enabled = false };
		public uint TowerEntityNetID
		{
			get { return _TowerEntityNetID; }
			set
			{
				// Don't do anything if the value is the same
				if (_TowerEntityNetID == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x1;
				_TowerEntityNetID = value;
				hasDirtyFields = true;
			}
		}

		public void SetTowerEntityNetIDDirty()
		{
			_dirtyFields[0] |= 0x1;
			hasDirtyFields = true;
		}

		private void RunChange_TowerEntityNetID(ulong timestep)
		{
			if (TowerEntityNetIDChanged != null) TowerEntityNetIDChanged(_TowerEntityNetID, timestep);
			if (fieldAltered != null) fieldAltered("TowerEntityNetID", _TowerEntityNetID, timestep);
		}

		protected override void OwnershipChanged()
		{
			base.OwnershipChanged();
			SnapInterpolations();
		}
		
		public void SnapInterpolations()
		{
			TowerEntityNetIDInterpolation.current = TowerEntityNetIDInterpolation.target;
		}

		public override int UniqueIdentity { get { return IDENTITY; } }

		protected override BMSByte WritePayload(BMSByte data)
		{
			UnityObjectMapper.Instance.MapBytes(data, _TowerEntityNetID);

			return data;
		}

		protected override void ReadPayload(BMSByte payload, ulong timestep)
		{
			_TowerEntityNetID = UnityObjectMapper.Instance.Map<uint>(payload);
			TowerEntityNetIDInterpolation.current = _TowerEntityNetID;
			TowerEntityNetIDInterpolation.target = _TowerEntityNetID;
			RunChange_TowerEntityNetID(timestep);
		}

		protected override BMSByte SerializeDirtyFields()
		{
			dirtyFieldsData.Clear();
			dirtyFieldsData.Append(_dirtyFields);

			if ((0x1 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _TowerEntityNetID);

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
				if (TowerEntityNetIDInterpolation.Enabled)
				{
					TowerEntityNetIDInterpolation.target = UnityObjectMapper.Instance.Map<uint>(data);
					TowerEntityNetIDInterpolation.Timestep = timestep;
				}
				else
				{
					_TowerEntityNetID = UnityObjectMapper.Instance.Map<uint>(data);
					RunChange_TowerEntityNetID(timestep);
				}
			}
		}

		public override void InterpolateUpdate()
		{
			if (IsOwner)
				return;

			if (TowerEntityNetIDInterpolation.Enabled && !TowerEntityNetIDInterpolation.current.UnityNear(TowerEntityNetIDInterpolation.target, 0.0015f))
			{
				_TowerEntityNetID = (uint)TowerEntityNetIDInterpolation.Interpolate();
				//RunChange_TowerEntityNetID(TowerEntityNetIDInterpolation.Timestep);
			}
		}

		private void Initialize()
		{
			if (readDirtyFlags == null)
				readDirtyFlags = new byte[1];

		}

		public TowerUIEntityNetworkObject() : base() { Initialize(); }
		public TowerUIEntityNetworkObject(NetWorker networker, INetworkBehavior networkBehavior = null, int createCode = 0, byte[] metadata = null) : base(networker, networkBehavior, createCode, metadata) { Initialize(); }
		public TowerUIEntityNetworkObject(NetWorker networker, uint serverId, FrameStream frame) : base(networker, serverId, frame) { Initialize(); }

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}
