import React from 'react';

function AddWaterModal({ visible, amount, onAmountChange, onConfirm, onCancel }) {
  if (!visible) return null;
  return (
    <div style={{
      position: 'fixed', top: 0, left: 0, width: '100vw', height: '100vh',
      background: 'rgba(0,0,0,0.2)', display: 'flex', alignItems: 'center', justifyContent: 'center', zIndex: 2000
    }}>
      <div style={{ background: '#fff', padding: 24, borderRadius: 8, minWidth: 300 }}>
        <h3>Add Water</h3>
        <input
          type="number"
          min={1}
          value={amount}
          onChange={e => onAmountChange(e.target.value)}
          style={{ width: 100, marginRight: 8 }}
        />
        <button onClick={onConfirm} style={{ marginRight: 8 }}>Confirm</button>
        <button onClick={onCancel}>Cancel</button>
      </div>
    </div>
  );
}

export default AddWaterModal; 