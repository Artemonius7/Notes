import React, {useState} from "react";
import "../css/Style.css"
import '../css/index.css';

interface NoteInputProps{
    onAddNote?: (text:string) => void;
}

export const NoteInput: React.FC<NoteInputProps> = ({onAddNote}) =>{
    const [value,setValue] = useState('');
    const handleKeyDown = (e:React.KeyboardEvent<HTMLInputElement>)=>{
        if (e.key == 'Enter' && value.trim())
        {
            onAddNote?.(value);
            setValue('');
        }
    };
    return(
        <div className="div-wrapper">
            <input
                type="text"
                className="text-wrapper form-control p-0"
                placeholder="Введите заметку..."
                value={value}
                onChange={(e)=> setValue(e.target.value)}
                onKeyDown={handleKeyDown}
                style={{
                    border: 'none',
                    outline:'none',
                    boxShadow:'none',
                    backgroundColor: 'transparent'
                }}
            />
        </div>
    );
}