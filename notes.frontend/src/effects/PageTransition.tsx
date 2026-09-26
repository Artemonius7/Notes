/* Мы добавили библиотеку Framer Motion для создания эффекта скольжения между страницами */
import React, {ReactNode} from 'react';
import {motion} from 'framer-motion';

interface PageTransitionProps{
    children: ReactNode;
}

export const PageTransition = ({children}: PageTransitionProps) =>{
    return (
        <motion.div
            initial={{opacity:0}}
            animate={{opacity:1}}
            exit={{opacity:0,}}
            transition={{duration:1, ease: "easeInOut"}}
            style={{width: '100%'}}
        >
            {children}
        </motion.div>
    );
};
export default PageTransition;

