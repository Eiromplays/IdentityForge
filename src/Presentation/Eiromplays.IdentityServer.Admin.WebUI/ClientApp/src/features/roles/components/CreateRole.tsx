import { Button, Form, FormDrawer, InputField } from 'eiromplays-ui';
import { HiOutlinePencil } from 'react-icons/hi';
import * as z from 'zod';

import { CreateRoleDTO, useCreateRole } from '../api/createRole';

const schema = z.object({
  name: z.string().min(1, 'Required'),
  description: z.string().nullable(),
});

export const CreateRole = () => {
  const createRoleMutation = useCreateRole();

  return (
    <>
      <FormDrawer
        isDone={createRoleMutation.isSuccess}
        triggerButton={
          <Button startIcon={<HiOutlinePencil className="h-4 w-4" />} size="sm">
            Create Role
          </Button>
        }
        title="Create Role"
        submitButton={
          <Button
            form="create-role"
            type="submit"
            size="sm"
            isLoading={createRoleMutation.isLoading}
          >
            Submit
          </Button>
        }
      >
        <Form<CreateRoleDTO['data'], typeof schema>
          id="create-role"
          onSubmit={async (values) => {
            await createRoleMutation.mutateAsync({ data: values });
          }}
          schema={schema}
        >
          {({ register, formState }) => (
            <>
              <InputField
                label="Name"
                error={formState.errors['name']}
                registration={register('name')}
              />
              <InputField
                label="Description"
                error={formState.errors['description']}
                registration={register('description')}
              />
            </>
          )}
        </Form>
      </FormDrawer>
    </>
  );
};
