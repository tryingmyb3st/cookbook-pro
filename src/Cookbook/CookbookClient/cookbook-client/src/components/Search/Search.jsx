import { useState, useRef, useEffect } from 'react';
import './Search.css';

export default function Search({ onSearch, onClear, placeholder, loading = false }) {
    const [searchTerm, setSearchTerm] = useState('');
    const [isFocused, setIsFocused] = useState(false);
    const inputRef = useRef(null);
    const timeoutRef = useRef(null);

    const handleInputChange = (e) => {
        const value = e.target.value;
        setSearchTerm(value);

        if (timeoutRef.current) {
            clearTimeout(timeoutRef.current);
        }

        if (value.trim()) {
            timeoutRef.current = setTimeout(() => {
                onSearch(value);
            }, 500);
        } else {
            onClear();
        }
    };

    const handleClear = () => {
        setSearchTerm('');
        onClear();
        inputRef.current.focus();
    };

    const handleKeyDown = (e) => {
        if (e.key === 'Enter' && searchTerm.trim()) {
            if (timeoutRef.current) {
                clearTimeout(timeoutRef.current);
            }
            onSearch(searchTerm);
        }
        if (e.key === 'Escape') {
            handleClear();
        }
    };

    useEffect(() => {
        return () => {
            if (timeoutRef.current) {
                clearTimeout(timeoutRef.current);
            }
        };
    }, []);

    return (
        <div className={`search-container ${isFocused ? 'focused' : ''}`}>
            <div className="search-input-wrapper">
                <input
                    ref={inputRef}
                    type="text"
                    className="search-input"
                    placeholder={placeholder}
                    value={searchTerm}
                    onChange={handleInputChange}
                    onKeyDown={handleKeyDown}
                    onFocus={() => setIsFocused(true)}
                    onBlur={() => setIsFocused(false)}
                    disabled={loading}
                />
                
                {searchTerm && (
                    <button 
                        className="clear-button" 
                        onClick={handleClear}
                        type="button"
                        aria-label="Очистить поиск"
                    >
                    </button>
                )}
                
                {loading && (
                    <div className="search-loading">
                        <div className="spinner"></div>
                    </div>
                )}
            </div>
        </div>
    );
}