import { Spinner, Table } from 'eiromplays-ui';

import { useUserClaims } from '../api/getUserClaims';
import { UserClaim } from '../types';

type UserRolesListProps = {
  id: string;
};

export const UserClaimsList = ({ id }: UserRolesListProps) => {
  const userClaimsQuery = useUserClaims({ userId: id });

  if (userClaimsQuery.isLoading) {
    return (
      <div className="w-full h-48 flex justify-center items-center">
        <Spinner size="lg" />
      </div>
    );
  }

  if (!userClaimsQuery?.data) return null;

  return (
    <>
      <Table<UserClaim>
        data={userClaimsQuery?.data || []}
        columns={[
          {
            title: 'Type',
            field: 'type',
          },
          {
            title: 'Value',
            field: 'value',
          },
          {
            title: 'ValueType',
            field: 'valueType',
          },
          {
            title: 'Issuer',
            field: 'issuer',
          },
        ]}
      />
    </>
  );
};
