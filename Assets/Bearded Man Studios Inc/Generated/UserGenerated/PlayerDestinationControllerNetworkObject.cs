using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedInterpol("{\"inter\":[0.15,0.15,0,0,0,0.15,0.15,0.15]")]
	public partial class PlayerDestinationControllerNetworkObject : NetworkObject
	{
		public const int IDENTITY = 7;

		private byte[] _dirtyFields = new byte[1];

		#pragma warning disable 0067
		public event FieldChangedEvent fieldAltered;
		#pragma warning restore 0067
		private Vector3 _destNetPosition;
		public event FieldEvent<Vector3> destNetPositionChanged;
		public InterpolateVector3 destNetPositionInterpolation = new InterpolateVector3() { LerpT = 0.15f, Enabled = true };
		public Vector3 destNetPosition
		{
			get { return _destNetPosition; }
			set
			{
				// Don't do anything if the value is the same
				if (_destNetPosition == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x1;
				_destNetPosition = value;
				hasDirtyFields = true;
			}
		}

		public void SetdestNetPositionDirty()
		{
			_dirtyFields[0] |= 0x1;
			hasDirtyFields = true;
		}

		private void RunChange_destNetPosition(ulong timestep)
		{
			if (destNetPositionChanged != null) destNetPositionChanged(_destNetPosition, timestep);
			if (fieldAltered != null) fieldAltered("destNetPosition", _destNetPosition, timestep);
		}
		private Quaternion _destNetRotation;
		public event FieldEvent<Quaternion> destNetRotationChanged;
		public InterpolateQuaternion destNetRotationInterpolation = new InterpolateQuaternion() { LerpT = 0.15f, Enabled = true };
		public Quaternion destNetRotation
		{
			get { return _destNetRotation; }
			set
			{
				// Don't do anything if the value is the same
				if (_destNetRotation == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x2;
				_destNetRotation = value;
				hasDirtyFields = true;
			}
		}

		public void SetdestNetRotationDirty()
		{
			_dirtyFields[0] |= 0x2;
			hasDirtyFields = true;
		}

		private void RunChange_destNetRotation(ulong timestep)
		{
			if (destNetRotationChanged != null) destNetRotationChanged(_destNetRotation, timestep);
			if (fieldAltered != null) fieldAltered("destNetRotation", _destNetRotation, timestep);
		}
		private Color _destNetColor;
		public event FieldEvent<Color> destNetColorChanged;
		public Interpolated<Color> destNetColorInterpolation = new Interpolated<Color>() { LerpT = 0f, Enabled = false };
		public Color destNetColor
		{
			get { return _destNetColor; }
			set
			{
				// Don't do anything if the value is the same
				if (_destNetColor == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x4;
				_destNetColor = value;
				hasDirtyFields = true;
			}
		}

		public void SetdestNetColorDirty()
		{
			_dirtyFields[0] |= 0x4;
			hasDirtyFields = true;
		}

		private void RunChange_destNetColor(ulong timestep)
		{
			if (destNetColorChanged != null) destNetColorChanged(_destNetColor, timestep);
			if (fieldAltered != null) fieldAltered("destNetColor", _destNetColor, timestep);
		}
		private Vector3 _destNetAvatorDims;
		public event FieldEvent<Vector3> destNetAvatorDimsChanged;
		public InterpolateVector3 destNetAvatorDimsInterpolation = new InterpolateVector3() { LerpT = 0f, Enabled = false };
		public Vector3 destNetAvatorDims
		{
			get { return _destNetAvatorDims; }
			set
			{
				// Don't do anything if the value is the same
				if (_destNetAvatorDims == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x8;
				_destNetAvatorDims = value;
				hasDirtyFields = true;
			}
		}

		public void SetdestNetAvatorDimsDirty()
		{
			_dirtyFields[0] |= 0x8;
			hasDirtyFields = true;
		}

		private void RunChange_destNetAvatorDims(ulong timestep)
		{
			if (destNetAvatorDimsChanged != null) destNetAvatorDimsChanged(_destNetAvatorDims, timestep);
			if (fieldAltered != null) fieldAltered("destNetAvatorDims", _destNetAvatorDims, timestep);
		}
		private Vector3 _destCursorDims;
		public event FieldEvent<Vector3> destCursorDimsChanged;
		public InterpolateVector3 destCursorDimsInterpolation = new InterpolateVector3() { LerpT = 0f, Enabled = false };
		public Vector3 destCursorDims
		{
			get { return _destCursorDims; }
			set
			{
				// Don't do anything if the value is the same
				if (_destCursorDims == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x10;
				_destCursorDims = value;
				hasDirtyFields = true;
			}
		}

		public void SetdestCursorDimsDirty()
		{
			_dirtyFields[0] |= 0x10;
			hasDirtyFields = true;
		}

		private void RunChange_destCursorDims(ulong timestep)
		{
			if (destCursorDimsChanged != null) destCursorDimsChanged(_destCursorDims, timestep);
			if (fieldAltered != null) fieldAltered("destCursorDims", _destCursorDims, timestep);
		}
		private float _destCursorSpeed;
		public event FieldEvent<float> destCursorSpeedChanged;
		public InterpolateFloat destCursorSpeedInterpolation = new InterpolateFloat() { LerpT = 0.15f, Enabled = true };
		public float destCursorSpeed
		{
			get { return _destCursorSpeed; }
			set
			{
				// Don't do anything if the value is the same
				if (_destCursorSpeed == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x20;
				_destCursorSpeed = value;
				hasDirtyFields = true;
			}
		}

		public void SetdestCursorSpeedDirty()
		{
			_dirtyFields[0] |= 0x20;
			hasDirtyFields = true;
		}

		private void RunChange_destCursorSpeed(ulong timestep)
		{
			if (destCursorSpeedChanged != null) destCursorSpeedChanged(_destCursorSpeed, timestep);
			if (fieldAltered != null) fieldAltered("destCursorSpeed", _destCursorSpeed, timestep);
		}
		private float _destAvatorSpeed;
		public event FieldEvent<float> destAvatorSpeedChanged;
		public InterpolateFloat destAvatorSpeedInterpolation = new InterpolateFloat() { LerpT = 0.15f, Enabled = true };
		public float destAvatorSpeed
		{
			get { return _destAvatorSpeed; }
			set
			{
				// Don't do anything if the value is the same
				if (_destAvatorSpeed == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x40;
				_destAvatorSpeed = value;
				hasDirtyFields = true;
			}
		}

		public void SetdestAvatorSpeedDirty()
		{
			_dirtyFields[0] |= 0x40;
			hasDirtyFields = true;
		}

		private void RunChange_destAvatorSpeed(ulong timestep)
		{
			if (destAvatorSpeedChanged != null) destAvatorSpeedChanged(_destAvatorSpeed, timestep);
			if (fieldAltered != null) fieldAltered("destAvatorSpeed", _destAvatorSpeed, timestep);
		}
		private float _destAvatorDestDist;
		public event FieldEvent<float> destAvatorDestDistChanged;
		public InterpolateFloat destAvatorDestDistInterpolation = new InterpolateFloat() { LerpT = 0.15f, Enabled = true };
		public float destAvatorDestDist
		{
			get { return _destAvatorDestDist; }
			set
			{
				// Don't do anything if the value is the same
				if (_destAvatorDestDist == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x80;
				_destAvatorDestDist = value;
				hasDirtyFields = true;
			}
		}

		public void SetdestAvatorDestDistDirty()
		{
			_dirtyFields[0] |= 0x80;
			hasDirtyFields = true;
		}

		private void RunChange_destAvatorDestDist(ulong timestep)
		{
			if (destAvatorDestDistChanged != null) destAvatorDestDistChanged(_destAvatorDestDist, timestep);
			if (fieldAltered != null) fieldAltered("destAvatorDestDist", _destAvatorDestDist, timestep);
		}

		protected override void OwnershipChanged()
		{
			base.OwnershipChanged();
			SnapInterpolations();
		}
		
		public void SnapInterpolations()
		{
			destNetPositionInterpolation.current = destNetPositionInterpolation.target;
			destNetRotationInterpolation.current = destNetRotationInterpolation.target;
			destNetColorInterpolation.current = destNetColorInterpolation.target;
			destNetAvatorDimsInterpolation.current = destNetAvatorDimsInterpolation.target;
			destCursorDimsInterpolation.current = destCursorDimsInterpolation.target;
			destCursorSpeedInterpolation.current = destCursorSpeedInterpolation.target;
			destAvatorSpeedInterpolation.current = destAvatorSpeedInterpolation.target;
			destAvatorDestDistInterpolation.current = destAvatorDestDistInterpolation.target;
		}

		public override int UniqueIdentity { get { return IDENTITY; } }

		protected override BMSByte WritePayload(BMSByte data)
		{
			UnityObjectMapper.Instance.MapBytes(data, _destNetPosition);
			UnityObjectMapper.Instance.MapBytes(data, _destNetRotation);
			UnityObjectMapper.Instance.MapBytes(data, _destNetColor);
			UnityObjectMapper.Instance.MapBytes(data, _destNetAvatorDims);
			UnityObjectMapper.Instance.MapBytes(data, _destCursorDims);
			UnityObjectMapper.Instance.MapBytes(data, _destCursorSpeed);
			UnityObjectMapper.Instance.MapBytes(data, _destAvatorSpeed);
			UnityObjectMapper.Instance.MapBytes(data, _destAvatorDestDist);

			return data;
		}

		protected override void ReadPayload(BMSByte payload, ulong timestep)
		{
			_destNetPosition = UnityObjectMapper.Instance.Map<Vector3>(payload);
			destNetPositionInterpolation.current = _destNetPosition;
			destNetPositionInterpolation.target = _destNetPosition;
			RunChange_destNetPosition(timestep);
			_destNetRotation = UnityObjectMapper.Instance.Map<Quaternion>(payload);
			destNetRotationInterpolation.current = _destNetRotation;
			destNetRotationInterpolation.target = _destNetRotation;
			RunChange_destNetRotation(timestep);
			_destNetColor = UnityObjectMapper.Instance.Map<Color>(payload);
			destNetColorInterpolation.current = _destNetColor;
			destNetColorInterpolation.target = _destNetColor;
			RunChange_destNetColor(timestep);
			_destNetAvatorDims = UnityObjectMapper.Instance.Map<Vector3>(payload);
			destNetAvatorDimsInterpolation.current = _destNetAvatorDims;
			destNetAvatorDimsInterpolation.target = _destNetAvatorDims;
			RunChange_destNetAvatorDims(timestep);
			_destCursorDims = UnityObjectMapper.Instance.Map<Vector3>(payload);
			destCursorDimsInterpolation.current = _destCursorDims;
			destCursorDimsInterpolation.target = _destCursorDims;
			RunChange_destCursorDims(timestep);
			_destCursorSpeed = UnityObjectMapper.Instance.Map<float>(payload);
			destCursorSpeedInterpolation.current = _destCursorSpeed;
			destCursorSpeedInterpolation.target = _destCursorSpeed;
			RunChange_destCursorSpeed(timestep);
			_destAvatorSpeed = UnityObjectMapper.Instance.Map<float>(payload);
			destAvatorSpeedInterpolation.current = _destAvatorSpeed;
			destAvatorSpeedInterpolation.target = _destAvatorSpeed;
			RunChange_destAvatorSpeed(timestep);
			_destAvatorDestDist = UnityObjectMapper.Instance.Map<float>(payload);
			destAvatorDestDistInterpolation.current = _destAvatorDestDist;
			destAvatorDestDistInterpolation.target = _destAvatorDestDist;
			RunChange_destAvatorDestDist(timestep);
		}

		protected override BMSByte SerializeDirtyFields()
		{
			dirtyFieldsData.Clear();
			dirtyFieldsData.Append(_dirtyFields);

			if ((0x1 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _destNetPosition);
			if ((0x2 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _destNetRotation);
			if ((0x4 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _destNetColor);
			if ((0x8 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _destNetAvatorDims);
			if ((0x10 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _destCursorDims);
			if ((0x20 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _destCursorSpeed);
			if ((0x40 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _destAvatorSpeed);
			if ((0x80 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _destAvatorDestDist);

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
				if (destNetPositionInterpolation.Enabled)
				{
					destNetPositionInterpolation.target = UnityObjectMapper.Instance.Map<Vector3>(data);
					destNetPositionInterpolation.Timestep = timestep;
				}
				else
				{
					_destNetPosition = UnityObjectMapper.Instance.Map<Vector3>(data);
					RunChange_destNetPosition(timestep);
				}
			}
			if ((0x2 & readDirtyFlags[0]) != 0)
			{
				if (destNetRotationInterpolation.Enabled)
				{
					destNetRotationInterpolation.target = UnityObjectMapper.Instance.Map<Quaternion>(data);
					destNetRotationInterpolation.Timestep = timestep;
				}
				else
				{
					_destNetRotation = UnityObjectMapper.Instance.Map<Quaternion>(data);
					RunChange_destNetRotation(timestep);
				}
			}
			if ((0x4 & readDirtyFlags[0]) != 0)
			{
				if (destNetColorInterpolation.Enabled)
				{
					destNetColorInterpolation.target = UnityObjectMapper.Instance.Map<Color>(data);
					destNetColorInterpolation.Timestep = timestep;
				}
				else
				{
					_destNetColor = UnityObjectMapper.Instance.Map<Color>(data);
					RunChange_destNetColor(timestep);
				}
			}
			if ((0x8 & readDirtyFlags[0]) != 0)
			{
				if (destNetAvatorDimsInterpolation.Enabled)
				{
					destNetAvatorDimsInterpolation.target = UnityObjectMapper.Instance.Map<Vector3>(data);
					destNetAvatorDimsInterpolation.Timestep = timestep;
				}
				else
				{
					_destNetAvatorDims = UnityObjectMapper.Instance.Map<Vector3>(data);
					RunChange_destNetAvatorDims(timestep);
				}
			}
			if ((0x10 & readDirtyFlags[0]) != 0)
			{
				if (destCursorDimsInterpolation.Enabled)
				{
					destCursorDimsInterpolation.target = UnityObjectMapper.Instance.Map<Vector3>(data);
					destCursorDimsInterpolation.Timestep = timestep;
				}
				else
				{
					_destCursorDims = UnityObjectMapper.Instance.Map<Vector3>(data);
					RunChange_destCursorDims(timestep);
				}
			}
			if ((0x20 & readDirtyFlags[0]) != 0)
			{
				if (destCursorSpeedInterpolation.Enabled)
				{
					destCursorSpeedInterpolation.target = UnityObjectMapper.Instance.Map<float>(data);
					destCursorSpeedInterpolation.Timestep = timestep;
				}
				else
				{
					_destCursorSpeed = UnityObjectMapper.Instance.Map<float>(data);
					RunChange_destCursorSpeed(timestep);
				}
			}
			if ((0x40 & readDirtyFlags[0]) != 0)
			{
				if (destAvatorSpeedInterpolation.Enabled)
				{
					destAvatorSpeedInterpolation.target = UnityObjectMapper.Instance.Map<float>(data);
					destAvatorSpeedInterpolation.Timestep = timestep;
				}
				else
				{
					_destAvatorSpeed = UnityObjectMapper.Instance.Map<float>(data);
					RunChange_destAvatorSpeed(timestep);
				}
			}
			if ((0x80 & readDirtyFlags[0]) != 0)
			{
				if (destAvatorDestDistInterpolation.Enabled)
				{
					destAvatorDestDistInterpolation.target = UnityObjectMapper.Instance.Map<float>(data);
					destAvatorDestDistInterpolation.Timestep = timestep;
				}
				else
				{
					_destAvatorDestDist = UnityObjectMapper.Instance.Map<float>(data);
					RunChange_destAvatorDestDist(timestep);
				}
			}
		}

		public override void InterpolateUpdate()
		{
			if (IsOwner)
				return;

			if (destNetPositionInterpolation.Enabled && !destNetPositionInterpolation.current.UnityNear(destNetPositionInterpolation.target, 0.0015f))
			{
				_destNetPosition = (Vector3)destNetPositionInterpolation.Interpolate();
				//RunChange_destNetPosition(destNetPositionInterpolation.Timestep);
			}
			if (destNetRotationInterpolation.Enabled && !destNetRotationInterpolation.current.UnityNear(destNetRotationInterpolation.target, 0.0015f))
			{
				_destNetRotation = (Quaternion)destNetRotationInterpolation.Interpolate();
				//RunChange_destNetRotation(destNetRotationInterpolation.Timestep);
			}
			if (destNetColorInterpolation.Enabled && !destNetColorInterpolation.current.UnityNear(destNetColorInterpolation.target, 0.0015f))
			{
				_destNetColor = (Color)destNetColorInterpolation.Interpolate();
				//RunChange_destNetColor(destNetColorInterpolation.Timestep);
			}
			if (destNetAvatorDimsInterpolation.Enabled && !destNetAvatorDimsInterpolation.current.UnityNear(destNetAvatorDimsInterpolation.target, 0.0015f))
			{
				_destNetAvatorDims = (Vector3)destNetAvatorDimsInterpolation.Interpolate();
				//RunChange_destNetAvatorDims(destNetAvatorDimsInterpolation.Timestep);
			}
			if (destCursorDimsInterpolation.Enabled && !destCursorDimsInterpolation.current.UnityNear(destCursorDimsInterpolation.target, 0.0015f))
			{
				_destCursorDims = (Vector3)destCursorDimsInterpolation.Interpolate();
				//RunChange_destCursorDims(destCursorDimsInterpolation.Timestep);
			}
			if (destCursorSpeedInterpolation.Enabled && !destCursorSpeedInterpolation.current.UnityNear(destCursorSpeedInterpolation.target, 0.0015f))
			{
				_destCursorSpeed = (float)destCursorSpeedInterpolation.Interpolate();
				//RunChange_destCursorSpeed(destCursorSpeedInterpolation.Timestep);
			}
			if (destAvatorSpeedInterpolation.Enabled && !destAvatorSpeedInterpolation.current.UnityNear(destAvatorSpeedInterpolation.target, 0.0015f))
			{
				_destAvatorSpeed = (float)destAvatorSpeedInterpolation.Interpolate();
				//RunChange_destAvatorSpeed(destAvatorSpeedInterpolation.Timestep);
			}
			if (destAvatorDestDistInterpolation.Enabled && !destAvatorDestDistInterpolation.current.UnityNear(destAvatorDestDistInterpolation.target, 0.0015f))
			{
				_destAvatorDestDist = (float)destAvatorDestDistInterpolation.Interpolate();
				//RunChange_destAvatorDestDist(destAvatorDestDistInterpolation.Timestep);
			}
		}

		private void Initialize()
		{
			if (readDirtyFlags == null)
				readDirtyFlags = new byte[1];

		}

		public PlayerDestinationControllerNetworkObject() : base() { Initialize(); }
		public PlayerDestinationControllerNetworkObject(NetWorker networker, INetworkBehavior networkBehavior = null, int createCode = 0, byte[] metadata = null) : base(networker, networkBehavior, createCode, metadata) { Initialize(); }
		public PlayerDestinationControllerNetworkObject(NetWorker networker, uint serverId, FrameStream frame) : base(networker, serverId, frame) { Initialize(); }

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}
