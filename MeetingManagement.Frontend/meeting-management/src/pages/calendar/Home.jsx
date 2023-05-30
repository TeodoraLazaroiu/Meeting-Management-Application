import React from 'react';
import { CalendarComponent } from '../../components/ui/CalendarComponent';
import { Navbar } from '../../components/ui/Navbar';

export const Home = () => {
    return (
        <div>
        <Navbar/>
        <CalendarComponent/>
        </div>
        
    )
}