import React from 'react';

function ConnectModal({ visible, candidates, onSelect, onCancel }) {
  if (!visible) return null;
  return (
    <div style={{
      position: 'fixed', top: 0, left: 0, width: '100vw', height: '100vh',
      background: 'rgba(0,0,0,0.2)', display: 'flex', alignItems: 'center', justifyContent: 'center', zIndex: 2000
    }}>
      <div style={{ background: '#fff', padding: 24, borderRadius: 8, minWidth: 300 }}>
        <h3>Choose Container</h3>
        <ul style={{ listStyle: 'none', padding: 0 }}>
          {candidates.map(c => (
            <li key={c.id} style={{ margin: '8px 0' }}>
              <button onClick={() => onSelect(c.id)} style={{ width: '100%', padding: 8 }}>
                {c.name}
              </button>
            </li>
          ))}
        </ul>
        <button onClick={onCancel} style={{ marginTop: 12 }}>Cancel</button>
      </div>
    </div>
  );
}

export default ConnectModal; 