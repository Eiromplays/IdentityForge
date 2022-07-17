import { MatchRoute, useMatch, useSearch } from '@tanstack/react-location';
import { ContentLayout, Link, Spinner, useAuth } from 'eiromplays-ui';
import React from 'react';

import { LocationGenerics } from '@/App';

import { useUser } from '../api/getUser';
import { UpdateUser } from '../components/UpdateUser';

type EntryProps = {
  label: string;
  value: string;
};

const Entry = ({ label, value }: EntryProps) => (
  <div className="py-4 sm:py-5 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-6">
    <dt className="text-sm font-medium text-gray-500 dark:text-white">{label}</dt>
    <dd className="mt-1 text-sm text-gray-900 dark:text-white sm:mt-0 sm:col-span-2">{value}</dd>
  </div>
);

const PictureEntry = ({ label, value }: EntryProps) => (
  <div className="py-4 sm:py-5 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-6">
    <dt className="text-sm font-medium text-gray-500 dark:text-white">{label}</dt>
    <dd className="mt-1 text-sm text-gray-900 dark:text-white sm:mt-0 sm:col-span-2">
      <img width="200" height="200" className="rounded-full" src={value} alt="" />
    </dd>
  </div>
);

export const User = () => {
  const { user } = useAuth();
  const search = useSearch<LocationGenerics>();

  const {
    params: { userId },
  } = useMatch<LocationGenerics>();

  const userQuery = useUser({ userId: userId });

  //TODO: Find a better way to do this
  if (user.id === userId) {
    window.location.href = window.location.href.replace(userId, '');
    return null;
  }

  if (userQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }
  if (!userQuery.data) return null;

  return (
    <ContentLayout title={`User ${userQuery.data?.userName}`}>
      <div className="bg-white dark:bg-gray-800 shadow overflow-hidden sm:rounded-lg">
        <div className="px-4 py-5 sm:px-6">
          <div className="flex justify-between">
            <h3 className="text-lg leading-6 font-medium text-gray-900 dark:text-gray-200">
              User Information
            </h3>
            <UpdateUser id={userId} />
            <Link to={`roles`} search={search} className="block">
              <pre className={`text-sm`}>
                Roles{' '}
                <MatchRoute to={`roles`} pending>
                  <Spinner size="md" className="inline-block" />
                </MatchRoute>
              </pre>
            </Link>
            <Link to={`claims`} search={search} className="block">
              <pre className={`text-sm`}>
                Claims{' '}
                <MatchRoute to={`claims`} pending>
                  <Spinner size="md" className="inline-block" />
                </MatchRoute>
              </pre>
            </Link>
          </div>
          <p className="mt-1 max-w-2xl text-sm text-gray-500 dark:text-white">
            Personal details of the user.
          </p>
        </div>
        <div className="border-t border-gray-200 px-4 py-5 sm:p-0">
          <dl className="sm:divide-y sm:divide-gray-200">
            <Entry label="Id" value={userQuery.data.id} />
            <Entry label="Username" value={userQuery.data.userName} />
            <Entry label="Display Name" value={userQuery.data.displayName} />
            <Entry label="First Name" value={userQuery.data.firstName} />
            <Entry label="Last Name" value={userQuery.data.lastName} />
            <Entry label="Email Address" value={userQuery.data.email} />
            <Entry label="Phone Number" value={userQuery.data.phoneNumber} />
            {userQuery.data.gravatarEmail && (
              <Entry label="Gravatar Email Address" value={userQuery.data.gravatarEmail} />
            )}
            {userQuery.data.profilePicture && (
              <PictureEntry label={'Profile Picture'} value={userQuery.data.profilePicture} />
            )}
            <Entry label="Email Confirmed" value={userQuery.data.emailConfirmed.toString()} />
            <Entry label="Is Active" value={userQuery.data.isActive.toString()} />
            <Entry
              label="Phone Number Confirmed"
              value={userQuery.data.phoneNumberConfirmed.toString()}
            />
            <Entry label="Lockout Enabled" value={userQuery.data.lockoutEnabled.toString()} />
            <Entry label="Two-factor Enabled" value={userQuery.data.twoFactorEnabled.toString()} />
          </dl>
        </div>
      </div>
    </ContentLayout>
  );
};
