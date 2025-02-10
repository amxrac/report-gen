document.addEventListener('DOMContentLoaded', () => {
    const themeSwitcher = document.getElementById('themeSwitcher');
    const darkIcon = document.getElementById('darkIcon');
    const lightIcon = document.getElementById('lightIcon');

    const toggleTheme = () => {
        const currentTheme = document.documentElement.getAttribute('data-bs-theme');
        const newTheme = currentTheme === 'dark' ? 'light' : 'dark';

        localStorage.setItem('theme', newTheme);

        document.documentElement.setAttribute('data-bs-theme', newTheme);

        darkIcon.style.display = newTheme === 'dark' ? 'none' : 'inline';
        lightIcon.style.display = newTheme === 'dark' ? 'inline' : 'none';
    };

    if (themeSwitcher) {
        themeSwitcher.addEventListener('click', toggleTheme);
    }

    const initializeTheme = () => {
        const savedTheme = localStorage.getItem('theme');
        const systemPreference = window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
        const theme = savedTheme || systemPreference;

        document.documentElement.setAttribute('data-bs-theme', theme);

        darkIcon.style.display = theme === 'dark' ? 'none' : 'inline';
        lightIcon.style.display = theme === 'dark' ? 'inline' : 'none';
    };

    initializeTheme();
});