import React from 'react';

function ContainerGraph({ containers, handleConnect, handleDelete, handleAddWater }) {
  const width = 160 * containers.length + 60;
  const height = 220;
  const containerWidth = 60;
  const containerHeight = 100;
  const marginX = 100;
  const marginY = 50;

  const positions = containers.map((c, i) => ({
    id: c.id,
    x: marginX + i * 130,
    y: marginY
  }));

  const getPos = id => positions.find(p => p.id === id);

  const handleDisconnect = async (id1, id2) => {
    if (!window.confirm('Are you sure you want to disconnect these two containers?')) return;
    try {
      const res = await fetch(`/api/containers/disconnections`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ sourceContainerId: id1, targetContainerId: id2 })
      });
      if (!res.ok) throw new Error('Disconnect failed');
      window.location.reload();
    } catch (e) {
      alert(e.message);
    }
  };

  return (
    <svg width={width} height={height} style={{ background: '#f8f8f8', borderRadius: 12, marginBottom: 32 }}>
      {containers.map((c, i) => (
        c.connectedContainerIds?.map(cid => {
          const from = getPos(c.id);
          const to = getPos(cid);
          if (!to || c.id > cid) return null;
          const mx = (from.x + containerWidth + to.x) / 2;
          const my = (from.y + containerHeight / 2 + to.y + containerHeight / 2) / 2;
          return (
            <g key={c.id + '-' + cid}>
              <line
                x1={from.x + containerWidth}
                y1={from.y + containerHeight / 2}
                x2={to.x}
                y2={to.y + containerHeight / 2}
                stroke="#222"
                strokeWidth={2}
              />
              <g
                style={{ cursor: 'pointer' }}
                onClick={() => handleDisconnect(c.id, cid)}
                transform={`translate(${mx - 10}, ${my - 10})`}
              >
                <rect width={20} height={20} rx={6} fill="#fff" stroke="#f44336" strokeWidth={1.5} />
                <line x1={5} y1={5} x2={15} y2={15} stroke="#f44336" strokeWidth={2} />
                <line x1={15} y1={5} x2={5} y2={15} stroke="#f44336" strokeWidth={2} />
              </g>
            </g>
          );
        })
      ))}
      {containers.map((c, i) => {
        const pos = getPos(c.id);
        const waterHeight = containerHeight * (c.amount / c.capacity);
        const isFull = c.amount >= c.capacity;
        const showWater = c.amount > 0;
        return (
          <g key={c.id}>
            <text x={pos.x + containerWidth / 2} y={pos.y - 10} textAnchor="middle" fontSize={14} fill="#666">Capacity: {c.capacity}</text>
            <rect x={pos.x} y={pos.y} width={containerWidth} height={containerHeight} rx={15} fill="#fff" stroke="#222" strokeWidth={2} />
            {showWater && (
              <rect
                x={pos.x}
                y={pos.y + containerHeight - waterHeight}
                width={containerWidth}
                height={waterHeight}
                fill="#90caf9"
                rx={isFull ? 15 : 0}
                ry={isFull ? 15 : 0}
              />
            )}
            <text x={pos.x + containerWidth / 2} y={pos.y + containerHeight / 2 + 6} textAnchor="middle" fontSize={18} fill="#222">{c.amount}</text>
            <text x={pos.x + containerWidth / 2} y={pos.y + containerHeight + 24} textAnchor="middle" fontSize={16}>{c.name}</text>
            {(() => {
              const buttonSize = 24;
              const buttonGap = 12;
              const buttonY = pos.y + containerHeight + 36;
              const buttonStartX = pos.x + containerWidth / 2 - (buttonSize * 1.5 + buttonGap);
              return (
                <g>
                  <g
                    style={{ cursor: 'pointer' }}
                    onClick={() => handleAddWater(c.id)}
                    transform={`translate(${buttonStartX}, ${buttonY})`}
                    onMouseEnter={e => e.currentTarget.querySelector('rect').setAttribute('fill', '#bbdefb')}
                    onMouseLeave={e => e.currentTarget.querySelector('rect').setAttribute('fill', '#e3f2fd')}
                  >
                    <rect width={buttonSize} height={buttonSize} rx={12} fill="#e3f2fd" />
                    <line x1="12" y1="6" x2="12" y2="18" stroke="#1976d2" strokeWidth="2" />
                    <line x1="6" y1="12" x2="18" y2="12" stroke="#1976d2" strokeWidth="2" />
                  </g>
                  <g
                    style={{ cursor: 'pointer' }}
                    onClick={() => handleDelete(c.id)}
                    transform={`translate(${buttonStartX + buttonSize + buttonGap}, ${buttonY})`}
                    onMouseEnter={e => e.currentTarget.querySelector('rect').setAttribute('fill', '#ffcdd2')}
                    onMouseLeave={e => e.currentTarget.querySelector('rect').setAttribute('fill', '#ffebee')}
                  >
                    <rect width={buttonSize} height={buttonSize} rx={12} fill="#ffebee" />
                    <rect x="8" y="12" width="8" height="7" rx={2} fill="#f44336" />
                    <rect x="10" y="7" width="4" height="5" rx={1} fill="#f44336" />
                    <line x1="11" y1="15" x2="13" y2="17" stroke="#fff" strokeWidth="1.2" />
                    <line x1="13" y1="15" x2="11" y2="17" stroke="#fff" strokeWidth="1.2" />
                  </g>
                  <g
                    style={{ cursor: 'pointer' }}
                    onClick={() => handleConnect && handleConnect(c.id)}
                    transform={`translate(${buttonStartX + (buttonSize + buttonGap) * 2}, ${buttonY})`}
                    onMouseEnter={e => e.currentTarget.querySelector('rect').setAttribute('fill', '#bbdefb')}
                    onMouseLeave={e => e.currentTarget.querySelector('rect').setAttribute('fill', '#e3f2fd')}
                  >
                    <rect width={buttonSize} height={buttonSize} rx={12} fill="#e3f2fd" />
                    <circle cx="9" cy="12" r="4" fill="#1976d2" />
                    <circle cx="15" cy="12" r="4" fill="#1976d2" />
                    <line x1="13" y1="12" x2="11" y2="12" stroke="#1976d2" strokeWidth="2" />
                  </g>
                </g>
              );
            })()}
          </g>
        );
      })}
    </svg>
  );
}

export default ContainerGraph; 