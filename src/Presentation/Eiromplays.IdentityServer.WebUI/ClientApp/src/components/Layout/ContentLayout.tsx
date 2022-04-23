import * as React from 'react';

import { Head } from '../Head';

type ContentLayoutProps = {
  children: React.ReactNode;
  title: string;
  subTitle?: string;
  logo?: string;
  description?: string;
};

export const ContentLayout = ({
  children,
  title,
  subTitle,
  logo,
  description,
}: ContentLayoutProps) => {
  return (
    <>
      <Head title={title} description={description} />
      <div className="py-6">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 md:px-">
          <h1 className="text-2xl font-semibold text-gray-900 dark:text-gray-200">{title}</h1>
          {subTitle && <h2 className="text-sm text-gray-800 dark:text-gray-100">{subTitle}</h2>}
          {logo && (
            <img
              src={logo}
              alt="Client Logo"
              className="object-cover h-50 w-96 mt-2 sm:rounded-lg"
            />
          )}
        </div>
        <div className="max-w-7xl mx-auto px-4 sm:px-6 md:px-8 dark:text-white mt-2">
          {children}
        </div>
      </div>
    </>
  );
};
