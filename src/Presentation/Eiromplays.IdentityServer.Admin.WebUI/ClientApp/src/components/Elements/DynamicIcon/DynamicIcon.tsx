// Original source code: https://codesandbox.io/s/dynamiciconload-react-icons-6imgv?file=/src/DynamicIcon.tsx

import loadable from '@loadable/component';
import { CSSProperties, SVGAttributes } from 'react';
import { IconContext } from 'react-icons';

export type DynamicIconProps = {
  icon: string;
  color?: string;
  size?: string;
  className?: string;
  style?: CSSProperties;
  attr?: SVGAttributes<SVGElement>;
};

export const DynamicIcon = ({
  icon,
  color = '',
  size = '',
  className = '',
  style,
  attr,
}: DynamicIconProps) => {
  const [library, iconComponent] = icon.split('/');

  if (!library || !iconComponent) return <div>Could Not Find Icon</div>;

  const lib = library.toLowerCase();

  const Icon = loadable(() => import(`react-icons/${lib}/index.js`), {
    resolveComponent: (el: JSX.Element) => el[iconComponent as keyof JSX.Element],
  });

  const value: IconContext = {
    color: color,
    size: size,
    className: className,
    style: style,
    attr: attr,
  };

  return (
    <IconContext.Provider value={value}>
      <Icon />
    </IconContext.Provider>
  );
};
