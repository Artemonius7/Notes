import React, { FC, ReactElement, useEffect, useState } from 'react';
import { CreateNoteDto, Client, NoteLookupDto } from '../api/api';
import { NoteInput } from '../components/NoteInput';
import { UserProfileMenu } from '../components/UserProfileMenu';
import '../css/Style.css';
import '../css/index.css';
import logoImg from "../images/Logo.png";
import EditIcon from "../images/pencil.svg";
import TrashIcon from "../images/trash.svg";

const getAuthenticatedClient = () => {
    const token = localStorage.getItem('token');
    return new Client("http://localhost:5237", {
        fetch(url: RequestInfo, init?: RequestInit): Promise<Response> {
            const headers = new Headers(init?.headers);
            if (token) {
                headers.set('Authorization', `Bearer ${token}`);
            }
            return window.fetch(url, {
                ...init,
                headers,
            });
        }
    });
};

export const NoteList: FC<{}> = (): ReactElement => {
    const [notes, setNotes] = useState<NoteLookupDto[] | undefined>(undefined);
    const [username, setUsername] = useState<string>("Thomas");
    const [openMenuNoteId, setOpenMenuNoteId] = useState<string | number | null>(null);

    const getNotes = async () => {
        try {
            const apiClient = getAuthenticatedClient();
            const noteListVm = await apiClient.getAll();
            setNotes(noteListVm.notes);
        } catch (error) {
            console.error('Ошибка при получении заметок:', error);
        }
    };

    useEffect(() => {
        getNotes();

        const token = localStorage.getItem('token');
        if (token) {
            try {
                const payload = JSON.parse(atob(token.split('.')[1]));
                const currentName = payload.name || payload.unique_name || payload.sub;
                if (currentName) {
                    setUsername(currentName);
                }
            } catch (error) {
                console.error('Ошибка при расшифровке токена:', error);
            }
        }
    }, []);

    const createNote = async (note: CreateNoteDto) => {
        try {
            const apiClient = getAuthenticatedClient();
            await apiClient.create(note);
            await getNotes();
        } catch (error) {
            console.error('Ошибка при создании заметки:', error);
        }
    };

    return (
        <div style={{ 
            minHeight: '100vh', 
            display: 'flex', 
            flexDirection: 'column', 
            alignItems: 'center', 
            justifyContent: 'space-between', 
            padding: '40px 20px 24px 20px', 
            boxSizing: 'border-box',
            backgroundColor: '#ffffff',
            position: 'relative'
        }}
        onClick={() => setOpenMenuNoteId(null)}
        >
            
            {/* Меню профиля в правом верхнем углу */}
            <div style={{ position: 'absolute', top: '24px', right: '32px' }}>
                <UserProfileMenu
                    username={username}
                    onLogout={() => {
                        localStorage.removeItem('token');
                        window.location.reload();
                    }}
                    onEditProfile={() => alert('На стадии разработки')}
                />
            </div>

            {/* Центральный блок контента */}
            <div style={{ 
                display: 'flex', 
                flexDirection: 'column', 
                alignItems: 'center', 
                width: '100%', 
                maxWidth: '621px',
                margin: 'auto'
            }}>
                
                <div style={{ marginBottom: '40px', display: 'flex', justifyContent: 'center' }}>
                    <img
                        src={logoImg}
                        alt='Notes Logo'
                        style={{ height: '64px', objectFit: 'contain' }}
                    />
                </div>
                
                {/* Поле ввода заметки */}
                <div style={{ width: '100%', marginBottom: '24px', display: 'flex', justifyContent: 'center' }}>
                    <NoteInput
                        onAddNote={async (text) => {
                            const note: CreateNoteDto = {
                                title: text,
                                details: ""
                            };  
                            await createNote(note);               
                        }}
                    />
                </div>
                
                <div style={{ 
                    fontSize: '32px',
                    fontFamily: 'SF Pro Display, sans-serif', 
                    fontWeight: 500, 
                    color: '#000000', 
                    marginBottom: '16px', 
                    width: '100%', 
                    textAlign: 'center' 
                }}>
                    Ваши заметки:
                </div>

                <div style={{ position: 'relative', width: '100%', display: 'flex', justifyContent: 'center' }}>
                    
                    <div style={{
                        border: '1px solid #c6c4c4',
                        borderRadius: '16px',
                        height: '240px',
                        width: '100%',
                        padding: '24px',
                        boxSizing: 'border-box',
                        display: 'flex',
                        flexDirection: 'column',
                        background: '#ffffff',
                        overflowY: 'auto'
                    }}>
                        <div style={{ display: 'flex', flexDirection: 'column', gap: '16px', width: '100%' }}>
                            {notes?.map((note: NoteLookupDto, index: number) => {
                                const noteId = note.id || index;
                                const isMenuOpen = openMenuNoteId === noteId;

                                return (
                                    <div 
                                        key={noteId}
                                        style={{ 
                                            width: '100%',
                                            backgroundColor: '#ffffff',
                                            display: 'flex',
                                            flexDirection: 'row',
                                            justifyContent: 'space-between',
                                            alignItems: 'center',
                                            paddingBottom: '8px',
                                            borderBottom: index < (notes.length - 1) ? '1px solid #f0f0f0' : 'none'
                                        }}
                                    >
                                        <span style={{ 
                                            fontSize: '22px',
                                            fontFamily: 'SF Pro Display, sans-serif', 
                                            fontWeight: 400,
                                            color: '#000000', 
                                            textAlign: 'left',
                                            wordBreak: 'break-word',
                                            paddingRight: '16px'
                                        }}>
                                            {note.title}
                                        </span>

                                        <div 
                                            onClick={(e) => {
                                                e.stopPropagation();
                                                setOpenMenuNoteId(isMenuOpen ? null : noteId);
                                            }}
                                            style={{ cursor: 'pointer', padding: '8px', userSelect: 'none', color: '#000000' }}
                                            title="Опции"
                                        >
                                            <svg width="4" height="18" viewBox="0 0 4 18" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
                                                <circle cx="2" cy="2" r="2"/>
                                                <circle cx="2" cy="9" r="2"/>
                                                <circle cx="2" cy="16" r="2"/>
                                            </svg>
                                        </div>
                                    </div>
                                );
                            })}
                        </div>
                    </div>

                    {openMenuNoteId !== null && (
                        <div style={{
                            position: 'absolute',
                            top: '20px',
                            right: '-185px',
                            background: '#ffffff',
                            border: '1px solid #c6c4c4',
                            borderRadius: '12px',
                            boxShadow: '0px 4px 16px rgba(0, 0, 0, 0.08)',
                            width: '170px',
                            zIndex: 50,
                            display: 'flex',
                            flexDirection: 'column',
                            padding: '12px',
                            boxSizing: 'border-box',
                            gap: '12px'
                        }}
                        onClick={(e) => e.stopPropagation()}
                        >
                            <div 
                                style={{ 
                                    display: 'flex', 
                                    alignItems: 'center', 
                                    cursor: 'pointer', 
                                    fontSize: '14px', 
                                    fontFamily: 'SF Pro Display, sans-serif',
                                    fontWeight: 400, 
                                    color: '#000000',
                                    gap: '10px' 
                                }}
                                onClick={() => {
                                    alert('Изменение заметки на стадии разработки');
                                    setOpenMenuNoteId(null);
                                }}
                            >
                                <img src={EditIcon} alt="edit" style={{ 
                                    width: '14px', 
                                    height: '14px', 
                                    objectFit: 'contain', }} />
                                <span>Изменить заметку</span>
                            </div>

                            <div 
                                style={{ 
                                    display: 'flex', 
                                    alignItems: 'center', 
                                    cursor: 'pointer', 
                                    fontSize: '14px', 
                                    fontFamily: 'SF Pro Display, sans-serif',
                                    fontWeight: 400, 
                                    color: '#000000',
                                    gap: '10px' 
                                }}
                                onClick={() => {
                                    alert('Удаление заметки на стадии разработки');
                                    setOpenMenuNoteId(null);
                                }}
                            >
                                <img src={TrashIcon} alt="delete" style={{ width: '14px', height: '14px', objectFit: 'contain' }} />
                                <span>Удалить</span>
                            </div>
                        </div>
                    )}
                </div>
            </div>

            <div style={{ 
                fontSize: '15px', 
                fontFamily: 'SF Pro Display, sans-serif',
                fontWeight: 400, 
                color: '#000000',
                textAlign: 'center',
                marginTop: '20px'
            }}>
                © 2026 Soprano Team. All rights reserved.
            </div>
            
        </div>    
    );
};
export default NoteList;