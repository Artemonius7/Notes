import React, { useState } from 'react';
import UserIcon from "../images/User.svg";
import EditIcon from "../images/pencil.svg";
import ChevronIcon from "../images/chevron_down.svg";
import LogoutIcon from "../images/exit.svg";
import '../css/index.css';

interface UserProfileMenuProps {
    username?: string;
    onEditProfile?: () => void;
    onLogout?: () => void;
}

export const UserProfileMenu: React.FC<UserProfileMenuProps> = ({ 
    username = "User", 
    onEditProfile, 
    onLogout 
}) => {
    const [isOpen, setIsOpen] = useState(false);

    return (
        <div style={{ position: 'relative' }}>
            <div 
                onClick={() => setIsOpen(!isOpen)}
                style={{ 
                    display: 'flex', 
                    alignItems: 'center', 
                    gap: '8px', 
                    cursor: 'pointer', 
                    userSelect: 'none' 
                }}
            >
                <img src={UserIcon} alt="user" style={{ width: '18px', height: '18px', objectFit: 'contain', flexShrink: 0 }} />
                
                <span style={{ color: '#4f5660', fontFamily: 'sans-serif', fontSize: '20px', fontWeight: 300 }}>
                    {username}
                </span>
                
                <span style={{ 
                    transform: isOpen ? 'rotate(180deg)' : 'rotate(0deg)', 
                    transition: 'transform 0.2s', 
                    display: 'inline-flex',
                    alignItems: 'center',
                    flexShrink: 0
                }}>
                    <img src={ChevronIcon} alt="chevron" style={{ width: '10px', height: '6px', objectFit: 'contain' }} />
                </span>
            </div>

            {isOpen && (
                <div style={{
                    position: 'absolute',
                    top: '100%',
                    right: 0,
                    marginTop: '12px',
                    background: '#ffffff',
                    border: '1px solid #c6c4c4',
                    borderRadius: '16px',
                    boxShadow: '0px 4px 16px rgba(0, 0, 0, 0.08)',
                    width: '210px',
                    zIndex: 100,
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'flex-start',
                    gap: '16px',
                    padding: '20px',
                    boxSizing: 'border-box'
                }}>
                    <div style={{ 
                        fontSize: '16px',
                        fontFamily: 'SF Pro Display, sans-serif', 
                        fontWeight: 600, 
                        color: '#000', 
                        width: '100%',
                        textAlign: 'center',
                        marginBottom: '4px' 
                    }}>
                        Профиль
                    </div>

                    <div 
                        style={{ 
                            display: 'flex', 
                            alignItems: 'center', 
                            gap: '12px', 
                            cursor: 'pointer', 
                            fontSize: '15px',
                            fontFamily: 'SF Pro Display, sans-serif', 
                            fontWeight: 400, 
                            color: '#000',
                            width: '100%'
                        }}
                        onClick={() => { setIsOpen(false); onEditProfile?.(); }}
                    >
                        <img src={EditIcon} alt="edit" style={{ width: '16px', height: '16px', objectFit: 'contain', flexShrink: 0 }} />
                        <span>Изменить профиль</span>
                    </div>

                    <div 
                        style={{ 
                            display: 'flex', 
                            alignItems: 'center', 
                            gap: '12px', 
                            cursor: 'pointer', 
                            fontSize: '15px',
                            fontFamily: 'SF Pro Display, sans-serif', 
                            fontWeight: 400, 
                            color: '#000',
                            width: '100%'
                        }}
                        onClick={() => { setIsOpen(false); onLogout?.(); }}
                    >
                        <img src={LogoutIcon} alt="logout" style={{ width: '16px', height: '16px', objectFit: 'contain', flexShrink: 0 }} />
                        <span>Выйти</span>
                    </div>
                </div>
            )}
        </div>
    );
};