using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedInterpol("{\"inter\":[0,0,0.15,0.15]")]
	public partial class PlayerAvatorControllerNetworkObject : NetworkObject
	{
		public const int IDENTITY = 7;

		private byte[] _dirtyFields = new byte[1];

		#pragma warning disable 0067
		public event FieldChangedEvent fieldAltered;
		#pragma warning restore 0067
		private uint _destNetwId;
		public event FieldEvent<uint> destNetwIdChanged;
		public Interpolated<uint> destNetwIdInterpolation = new Interpolated<uint>() { LerpT = 0f, Enabled = false };
		public uint destNetwId
		{
			get { return _destNetwId; }
			set
			{
				// Don't do anything if the value is the same
				if (_destNetwId == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x1;
				_destNetwId = value;
				hasDirtyFields = true;
			}
		}

		public void SetdestNetwIdDirty()
		{
			_dirtyFields[0] |= 0x1;
			hasDirtyFields = true;
		}

		private void RunChange_destNetwId(ulong timestep)
		{
			if (destNetwIdChanged != null) destNetwIdChanged(_destNetwId, timestep);
			if (fieldAltered != null) fieldAltered("destNetwId", _destNetwId, timestep);
		}
		private Vector3 _avatorNetDims;
		public event FieldEvent<Vector3> avatorNetDimsChanged;
		public InterpolateVector3 avatorNetDimsInterpolation = new InterpolateVector3() { LerpT = 0f, Enabled = false };
		public Vector3 avatorNetDims
		{
			get { return _avatorNetDims; }
			set
			{
				// Don't do anything if the value is the same
				if (_avatorNetDims == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x2;
				_avatorNetDims = value;
				hasDirtyFields = true;
			}
		}

		public void SetavatorNetDimsDirty()
		{
			_dirtyFields[0] |= 0x2;
			hasDirtyFields = true;
		}

		private void RunChange_avatorNetDims(ulong timestep)
		{
			if (avatorNetDimsChanged != null) avatorNetDimsChanged(_avatorNetDims, timestep);
			if (fieldAltered != null) fieldAltered("avatorNetDims", _avatorNetDims, timestep);
		}
		private float _avatorNetSpeed;
		public event FieldEvent<float> avatorNetSpeedChanged;
		public InterpolateFloat avatorNetSpeedInterpolation = new InterpolateFloat() { LerpT = 0.15f, Enabled = true };
		public float avatorNetSpeed
		{
			get { return _avatorNetSpeed; }
			set
			{
				// Don't do anything if the value is the same
				if (_avatorNetSpeed == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x4;
				_avatorNetSpeed = value;
				hasDirtyFields = true;
			}
		}

		public void SetavatorNetSpeedDirty()
		{
			_dirtyFields[0] |= 0x4;
			hasDirtyFields = true;
		}

		private void RunChange_avatorNetSpeed(ulong timestep)
		{
			if (avatorNetSpeedChanged != null) avatorNetSpeedChanged(_avatorNetSpeed, timestep);
			if (fieldAltered != null) fieldAltered("avatorNetSpeed", _avatorNetSpeed, timestep);
		}
		private float _avatorNetDestDistance;
		public event FieldEvent<float> avatorNetDestDistanceChanged;
		public InterpolateFloat avatorNetDestDistanceInterpolation = new InterpolateFloat() { LerpT = 0.15f, Enabled = true };
		public float avatorNetDestDistance
		{
			get { return _avatorNetDestDistance; }
			set
			{
				// Don't do anything if the value is the same
				if (_avatorNetDestDistance == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x8;
				_avatorNetDestDistance = value;
				hasDirtyFields = true;
			}
		}

		public void SetavatorNetDestDistanceDirty()
		{
			_dirtyFields[0] |= 0x8;
			hasDirtyFields = true;
		}

		private void RunChange_avatorNetDestDistance(ulong timestep)
		{
			if (avatorNetDestDistanceChanged != null) avatorNetDestDistanceChanged(_avatorNetDestDistance, timestep);
			if (fieldAltered != null) fieldAltered("avatorNetDestDistance", _avatorNetDestDistance, timestep);
		}

		protected override void OwnershipChanged()
		{
			base.OwnershipChanged();
			SnapInterpolations();
		}
		
		public void SnapInterpolations()
		{
			destNetwIdInterpolation.current = destNetwIdInterpolation.target;
			avatorNetDimsInterpolation.current = avatorNetDimsInterpolation.target;
			avatorNetSpeedInterpolation.current = avatorNetSpeedInterpolation.target;
			avatorNetDestDistanceInterpolation.current = avatorNetDestDistanceInterpolation.target;
		}

		public override int UniqueIdentity { get { return IDENTITY; } }

		protected override BMSByte WritePayload(BMSByte data)
		{
			UnityObjectMapper.Instance.MapBytes(data, _destNetwId);
			UnityObjectMapper.Instance.MapBytes(data, _avatorNetDims);
			UnityObjectMapper.Instance.MapBytes(data, _avatorNetSpeed);
			UnityObjectMapper.Instance.MapBytes(data, _avatorNetDestDistance);

			return data;
		}

		protected override void ReadPayload(BMSByte payload, ulong timestep)
		{
			_destNetwId = UnityObjectMapper.Instance.Map<uint>(payload);
			destNetwIdInterpolation.current = _destNetwId;
			destNetwIdInterpolation.target = _destNetwId;
			RunChange_destNetwId(timestep);
			_avatorNetDims = UnityObjectMapper.Instance.Map<Vector3>(payload);
			avatorNetDimsInterpolation.current = _avatorNetDims;
			avatorNetDimsInterpolation.target = _avatorNetDims;
			RunChange_avatorNetDims(timestep);
			_avatorNetSpeed = UnityObjectMapper.Instance.Map<float>(payload);
			avatorNetSpeedInterpolation.current = _avatorNetSpeed;
			avatorNetSpeedInterpolation.target = _avatorNetSpeed;
			RunChange_avatorNetSpeed(timestep);
			_avatorNetDestDistance = UnityObjectMapper.Instance.Map<float>(payload);
			avatorNetDestDistanceInterpolation.current = _avatorNetDestDistance;
			avatorNetDestDistanceInterpolation.target = _avatorNetDestDistance;
			RunChange_avatorNetDestDistance(timestep);
		}

		protected override BMSByte SerializeDirtyFields()
		{
			dirtyFieldsData.Clear();
			dirtyFieldsData.Append(_dirtyFields);

			if ((0x1 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _destNetwId);
			if ((0x2 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _avatorNetDims);
			if ((0x4 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _avatorNetSpeed);
			if ((0x8 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _avatorNetDestDistance);

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
				if (destNetwIdInterpolation.Enabled)
				{
					destNetwIdInterpolation.target = UnityObjectMapper.Instance.Map<uint>(data);
					destNetwIdInterpolation.Timestep = timestep;
				}
				else
				{
					_destNetwId = UnityObjectMapper.Instance.Map<uint>(data);
					RunChange_destNetwId(timestep);
				}
			}
			if ((0x2 & readDirtyFlags[0]) != 0)
			{
				if (avatorNetDimsInterpolation.Enabled)
				{
					avatorNetDimsInterpolation.target = UnityObjectMapper.Instance.Map<Vector3>(data);
					avatorNetDimsInterpolation.Timestep = timestep;
				}
				else
				{
					_avatorNetDims = UnityObjectMapper.Instance.Map<Vector3>(data);
					RunChange_avatorNetDims(timestep);
				}
			}
			if ((0x4 & readDirtyFlags[0]) != 0)
			{
				if (avatorNetSpeedInterpolation.Enabled)
				{
					avatorNetSpeedInterpolation.target = UnityObjectMapper.Instance.Map<float>(data);
					avatorNetSpeedInterpolation.Timestep = timestep;
				}
				else
				{
					_avatorNetSpeed = UnityObjectMapper.Instance.Map<float>(data);
					RunChange_avatorNetSpeed(timestep);
				}
			}
			if ((0x8 & readDirtyFlags[0]) != 0)
			{
				if (avatorNetDestDistanceInterpolation.Enabled)
				{
					avatorNetDestDistanceInterpolation.target = UnityObjectMapper.Instance.Map<float>(data);
					avatorNetDestDistanceInterpolation.Timestep = timestep;
				}
				else
				{
					_avatorNetDestDistance = UnityObjectMapper.Instance.Map<float>(data);
					RunChange_avatorNetDestDistance(timestep);
				}
			}
		}

		public override void InterpolateUpdate()
		{
			if (IsOwner)
				return;

			if (destNetwIdInterpolation.Enabled && !destNetwIdInterpolation.current.UnityNear(destNetwIdInterpolation.target, 0.0015f))
			{
				_destNetwId = (uint)destNetwIdInterpolation.Interpolate();
				//RunChange_destNetwId(destNetwIdInterpolation.Timestep);
			}
			if (avatorNetDimsInterpolation.Enabled && !avatorNetDimsInterpolation.current.UnityNear(avatorNetDimsInterpolation.target, 0.0015f))
			{
				_avatorNetDims = (Vector3)avatorNetDimsInterpolation.Interpolate();
				//RunChange_avatorNetDims(avatorNetDimsInterpolation.Timestep);
			}
			if (avatorNetSpeedInterpolation.Enabled && !avatorNetSpeedInterpolation.current.UnityNear(avatorNetSpeedInterpolation.target, 0.0015f))
			{
				_avatorNetSpeed = (float)avatorNetSpeedInterpolation.Interpolate();
				//RunChange_avatorNetSpeed(avatorNetSpeedInterpolation.Timestep);
			}
			if (avatorNetDestDistanceInterpolation.Enabled && !avatorNetDestDistanceInterpolation.current.UnityNear(avatorNetDestDistanceInterpolation.target, 0.0015f))
			{
				_avatorNetDestDistance = (float)avatorNetDestDistanceInterpolation.Interpolate();
				//RunChange_avatorNetDestDistance(avatorNetDestDistanceInterpolation.Timestep);
			}
		}

		private void Initialize()
		{
			if (readDirtyFlags == null)
				readDirtyFlags = new byte[1];

		}

		public PlayerAvatorControllerNetworkObject() : base() { Initialize(); }
		public PlayerAvatorControllerNetworkObject(NetWorker networker, INetworkBehavior networkBehavior = null, int createCode = 0, byte[] metadata = null) : base(networker, networkBehavior, createCode, metadata) { Initialize(); }
		public PlayerAvatorControllerNetworkObject(NetWorker networker, uint serverId, FrameStream frame) : base(networker, serverId, frame) { Initialize(); }

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}
