import { FaSun, FaMoon } from 'react-icons/fa';

import useDarkMode from '@/hooks/useDarkMode';

const ThemeToggle = () => {
  const { colorTheme, toggleTheme } = useDarkMode();

  return (
    <div className="transition duration-500 ease-in-out rounded-full p-2">
      {colorTheme === 'dark' ? (
        <FaSun
          onClick={toggleTheme}
          className="text-gray-500 dark:text-gray-400 text-2xl cursor-pointer"
        />
      ) : (
        <FaMoon
          onClick={toggleTheme}
          className="text-gray-500 dark:text-gray-400 text-2xl cursor-pointer"
        />
      )}
    </div>
  );
};

export default ThemeToggle;
