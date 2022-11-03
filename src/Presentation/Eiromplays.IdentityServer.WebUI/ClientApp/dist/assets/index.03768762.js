import{u as w,b as p,W as i,c,i as y,o as x,s as m,d as R,e as E,a,F as h,f as S,B as l,H as U,g as N,h as P,j as o,I as u,k as D,P as j,l as I,C as f,m as C,q as A,S as k,R as T,n as M,p as z,r as B}from"./index.43de77d9.js";const q=e=>new Promise((t,s)=>{const r=new FileReader;r.readAsDataURL(e),r.onload=()=>t(r.result),r.onerror=n=>s(n)}),$=async({data:e})=>{if(e.image instanceof File){const t=`.${e.image.name.slice((e.image.name.lastIndexOf(".")-1>>>0)+2)}`;e.image={data:await q(e.image),extension:t,name:e.image.name.replace(t,"")},e.deleteCurrentImage=e.image?!0:e.deleteCurrentImage}return c.put(`${y}/api/v1/manage/update-profile`,e)},Q=({config:e}={})=>{const{logout:t}=w();return{updateProfileMutation:p({onSuccess:async r=>{console.log(r),i.success(r.message),r.logoutRequired?(await t(),window.location.href=r.returnUrl):window.location.href=`/bff/login?returnUrl=${window.location.pathname}`},onError:r=>{var n;i.error("Failed to update user"),i.error((n=r.response)==null?void 0:n.data)},...e,mutationFn:$})}},_=x({firstName:m().min(1,"Required"),lastName:m().min(1,"Required"),email:m().min(1,"Required"),gravatarEmail:m(),deleteCurrentImage:R(),phoneNumber:m().nullable().refine(e=>e?E(e):!0,"Invalid phone number")}),O=()=>{const{user:e}=w();let t=[],s;const{updateProfileMutation:r}=Q();return a(h,{children:a(S,{isDone:r.isSuccess,triggerButton:a(l,{startIcon:a(U,{className:"h-4 w-4"}),size:"sm",children:"Update Profile"}),title:"Update Profile",submitButton:a(N,{icon:"warning",title:"Update Profile",body:"Are you sure you want to update your profile? This might require you to re-login.",triggerButton:a(l,{size:"sm",isLoading:r.isLoading,children:"Submit"}),confirmButton:a(l,{form:"update-profile",type:"submit",className:"mt-2",variant:"warning",size:"sm",isLoading:r.isLoading,children:"Update Profile"})}),children:a(P,{id:"update-profile",onSubmit:async n=>{n.image=s,n.id=e.id,await r.mutateAsync({data:n})},options:{defaultValues:{username:e==null?void 0:e.username,firstName:e==null?void 0:e.firstName,lastName:e==null?void 0:e.lastName,email:e==null?void 0:e.email,gravatarEmail:e==null?void 0:e.gravatarEmail,phoneNumber:e==null?void 0:e.phone_number,deleteCurrentImage:!1}},schema:_,onChange:(n,d)=>t=d,children:({register:n,formState:d,control:b})=>o(h,{children:[a(u,{label:"First Name",error:d.errors.firstName,registration:n("firstName")}),a(u,{label:"Last Name",error:d.errors.lastName,registration:n("lastName")}),a(u,{label:"Email Address",type:"email",error:d.errors.email,registration:n("email")}),a(u,{label:"Gravatar Email Address",type:"email",error:d.errors.gravatarEmail,registration:n("gravatarEmail")}),a(D,{label:"Phone Number",error:d.errors.phoneNumber,customInputField:a(j,{className:"bg-white dark:bg-gray-900 block px-3 py-2 border border-gray-300 dark:border-gray-700 rounded-md shadow-sm', 'placeholder-gray-400 dark:placeholder-white focus:outline-none focus:ring-blue-500 dark:focus:ring-indigo-700 focus:border-blue-500', 'dark:focus:border-indigo-900 sm:text-sm",name:"phoneNumber",control:b,register:n("phoneNumber")})}),a(u,{label:"Profile Picture",type:"file",accept:"image/*",error:d.errors.image,registration:n("image")}),(e==null?void 0:e.profilePicture)&&a(u,{label:"Delete profile picture",type:"checkbox",error:d.errors.deleteCurrentImage,registration:n("deleteCurrentImage")}),t&&t.length>0&&t[0].length>0&&a(h,{children:a("div",{className:"py-4 sm:py-5 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-6",children:a(I,{cropLabel:"Crop:",previewLabel:"Preview:",image:URL.createObjectURL(t[0][0]),onFileCreated:L=>{s=L}})})})]})})})})},g=({label:e,value:t})=>o("div",{className:"py-4 sm:py-5 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-6",children:[a("dt",{className:"text-sm font-medium text-gray-500 dark:text-white",children:e}),a("dd",{className:"mt-1 text-sm text-gray-900 dark:text-white sm:mt-0 sm:col-span-2",children:t})]}),G=({label:e,value:t})=>o("div",{className:"py-4 sm:py-5 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-6",children:[a("dt",{className:"text-sm font-medium text-gray-500 dark:text-white",children:e}),a("dd",{className:"mt-1 text-sm text-gray-900 dark:text-white sm:mt-0 sm:col-span-2",children:a("img",{width:"200",height:"200",className:"rounded-full",src:t,alt:""})})]}),Fe=()=>{const{user:e}=w();return e?a(f,{title:"Profile",children:o("div",{className:"bg-white dark:bg-gray-800 shadow overflow-hidden sm:rounded-lg",children:[o("div",{className:"px-4 py-5 sm:px-6",children:[o("div",{className:"flex justify-between",children:[a("h3",{className:"text-lg leading-6 font-medium text-gray-900 dark:text-gray-200",children:"User Information"}),a(O,{})]}),a("p",{className:"mt-1 max-w-2xl text-sm text-gray-500 dark:text-white",children:"Personal details of the user."})]}),a("div",{className:"border-t border-gray-200 px-4 py-5 sm:p-0",children:o("dl",{className:"sm:divide-y sm:divide-gray-200",children:[a(g,{label:"Id",value:e.id}),a(g,{label:"Username",value:e.username}),e.firstName&&a(g,{label:"First Name",value:e.firstName}),e.lastName&&a(g,{label:"Last Name",value:e.lastName}),e.email&&a(g,{label:"Email Address",value:e.email}),e.phone_number&&a(g,{label:"PhoneNumber",value:e.phone_number}),e.updated_at&&a(g,{label:"Last updated at",value:e.updated_at.toString()}),e.created_at&&a(g,{label:"Created at",value:e.created_at.toString()}),e.gravatarEmail&&a(g,{label:"Gravatar Email Address",value:e.gravatarEmail}),e.profilePicture&&a(G,{label:"Profile Picture",value:e.profilePicture}),e.roles.length>0&&a(g,{label:e.roles.length>1?"Roles":"Role",value:e.roles.join(", ")})]})})]})}):null},Y=()=>c.delete("/personal/profile"),H=({config:e}={})=>p({onError:()=>{i.error("Unable to delete user")},onSuccess:()=>{i.success("User Deleted")},...e,mutationFn:Y}),K=()=>c.get("/personal/export-personal-data",{headers:{"Content-Disposition":"attachment; filename=PersonalData.csv","Content-Type":"application/octet-stream"},responseType:"arraybuffer"}),V=({config:e}={})=>p({onSuccess:t=>{var n;const s=window.URL.createObjectURL(new Blob([t])),r=document.createElement("a");r.href=s,r.setAttribute("download","PersonalData.csv"),document.body.appendChild(r),r.click(),(n=r.parentNode)==null||n.removeChild(r),i.success("Exported Personal Data")},...e,mutationFn:K}),Le=()=>{const{user:e}=w(),t=V(),s=H();return e?a(f,{title:"Personal Data",children:o("div",{className:"bg-white dark:bg-gray-800 shadow overflow-hidden sm:rounded-lg",children:[o("div",{className:"px-4 py-5 sm:px-6",children:[a("div",{className:"flex justify-between",children:a("h3",{className:"text-lg leading-6 font-medium text-gray-900 dark:text-gray-200",children:"Personal Data"})}),a("p",{className:"mt-1 max-w-2xl text-sm text-gray-500 dark:text-white",children:"Personal data and information."})]}),o("div",{className:"border-t border-gray-200 flex gap-5 pt-5 pl-5 pb-5",children:[a(N,{icon:"danger",title:"Delete Account",body:"Are you sure you want to delete your account? There is no way to undo this action!",triggerButton:a(l,{size:"sm",variant:"danger",isLoading:s.isLoading,children:"Delete Account"}),confirmButton:a(l,{variant:"danger",size:"sm",isLoading:s.isLoading,onClick:async()=>{await s.mutateAsync(void 0),e!=null&&e.logoutUrl&&window.location.assign(e.logoutUrl)},children:"Proceed"})}),a(l,{variant:"primary",size:"sm",isLoading:t.isLoading,onClick:async()=>await t.mutateAsync(void 0),children:"Download Personal Data"})]})]})}):null},W=()=>c.get(`${y}/api/v1/manage/two-factor-authentication`),J=({config:e}={})=>C({...e,queryKey:["two-factor-authentication"],queryFn:()=>W()}),X=e=>c.post(`${y}/api/v1/manage/two-factor-authentication/enable`,e),Z=({config:e}={})=>p({onSuccess:async()=>{await A.invalidateQueries(["two-factor-authentication"]),i.success("Authenticator added successfully")},...e,mutationFn:X}),ee=()=>c.get(`${y}/api/v1/manage/two-factor-authentication/enable`),ae=({config:e}={})=>C({...e,queryKey:["get-enable-authenticator"],queryFn:()=>ee()}),te=()=>{const e=ae();return e.isLoading?a("div",{className:"w-full h-48 flex justify-center items-center",children:a(k,{size:"lg"})}):e.data?a(h,{children:a("div",{className:"bg-white dark:bg-gray-800 shadow overflow-hidden sm:rounded-lg",children:a("div",{className:"px-4 py-5 sm:px-6",children:o("p",{className:"mt-1 max-w-2xl text-sm text-gray-500 dark:text-white",children:["Scan this QRCode or use this key"," ",a("code",{className:"text-green-500",children:e.data.sharedKey})]})})})}):null},F=({codes:e})=>{const{user:t}=w();return!e||!t?null:a(f,{title:"Recovery Codes",children:o("div",{className:"bg-white dark:bg-gray-800 shadow overflow-hidden sm:rounded-lg",children:[o("div",{className:"px-4 py-5 sm:px-6",children:[a("div",{className:"flex justify-between",children:a("h3",{className:"text-lg leading-6 font-medium text-red-800 dark:text-red-600",children:"Make sure you store these codes in a safe place"})}),a("p",{className:"mt-1 max-w-2xl text-sm text-red-700 dark:text-red-500",children:"If you lose your device, or access to your recovery codes you will loose access to your account"})]}),e.map(s=>a("p",{className:"text-gray-800 dark:text-white pl-10 pb-4",children:s},s))]})})},re=x({code:m().optional()}),se=({provider:e})=>{var s,r,n,d;const t=Z();return o(h,{children:[(e==null?void 0:e.toLowerCase())==="app"&&a(te,{}),e&&(((s=t.data)==null?void 0:s.recoveryCodes)||[]).length<=0&&a("div",{className:"flex flex-column flex-wrap gap-5 pl-5 pb-5",children:a(P,{id:"add-authenticator",onSubmit:async b=>{b.provider=e,await t.mutateAsync(b)},options:{defaultValues:{code:""}},schema:re,children:()=>a(h,{children:a(l,{className:"mt-5",form:"add-authenticator",type:"submit",size:"sm",isLoading:t.isLoading,children:"Add Authenticator"})})})}),t.data&&((n=(r=t.data)==null?void 0:r.recoveryCodes)==null?void 0:n.length)>0&&a(F,{codes:(d=t.data)==null?void 0:d.recoveryCodes})]})},ne=[{value:"App",label:"App"}],oe=({options:e=[]})=>{const[t,s]=T.useState("App"),{currentTheme:r}=M();return ne.forEach(n=>{e.includes(n)||e.push(n)}),a(h,{children:o("div",{className:"flex flex-col gap-52",children:[(e==null?void 0:e.length)>0&&a(z,{options:e,onChange:n=>s((n==null?void 0:n.value)||""),value:{value:t,label:t},theme:n=>r==="dark"?{...n,colors:{...n.colors,primary:"#0a0e17",primary25:"gray",primary50:"#fff",neutral0:"#0a0e17"}}:{...n,colors:{...n.colors}}}),a(N,{icon:"info",title:"Add Authenticator",body:"You will need to add an authenticator to your account to use this feature.",triggerButton:a(l,{startIcon:a(B,{}),size:"sm",variant:"primary",children:"Add Authenticator"}),showCancelButton:!1,confirmButton:a(se,{provider:t})})]})})},ie=()=>c.post(`${y}/api/v1/manage/two-factor-authentication/disable`),le=({config:e}={})=>p({onSuccess:async()=>{await A.invalidateQueries(["two-factor-authentication"]),i.success("Authenticator disabled successfully")},...e,mutationFn:ie}),de=()=>{const e=le();return a(l,{onClick:async()=>await e.mutateAsync(void 0),isLoading:e.isLoading,variant:"inverse",size:"sm",children:"Disable Authenticator"})},ce=()=>c.post(`${y}/api/v1/manage/two-factor-authentication/forget`),ue=({config:e}={})=>p({onSuccess:async()=>{await A.invalidateQueries(["two-factor-authentication"]),i.success("The current browser has been forgotten. Next time you login from this browser you will be prompted for a 2fa code.")},...e,mutationFn:ce}),me=()=>{const e=ue();return a(l,{onClick:async()=>await e.mutateAsync(void 0),isLoading:e.isLoading,variant:"primary",size:"sm",children:"Forget this browser/machine"})},he=()=>c.post(`${y}/api/v1/manage/two-factor-authentication/generate-recovery-codes`),ge=({config:e}={})=>p({onSuccess:async()=>{await A.invalidateQueries(["two-factor-authentication"]),i.success("Successfully reset authentication app key.")},...e,mutationFn:he}),v=()=>{const e=ge();return o(h,{children:[a(l,{onClick:async()=>await e.mutateAsync(void 0),isLoading:e.isLoading,variant:"danger",size:"sm",children:"Reset recovery code"}),e.data&&e.data.length>0&&a(F,{codes:e.data})]})},pe=()=>c.post(`${y}/api/v1/manage/two-factor-authentication/reset`),we=({config:e}={})=>p({onSuccess:()=>{i.success("Successfully reset authentication app key.")},...e,mutationFn:pe}),ye=()=>{const e=we();return a(l,{onClick:async()=>await e.mutateAsync(void 0),isLoading:e.isLoading,variant:"danger",size:"sm",children:"Reset Authenticator"})},Re=()=>{const{user:e}=w(),t=J();return t.isLoading?a("div",{className:"w-full h-48 flex justify-center items-center",children:a(k,{size:"lg"})}):!t.data||!e?null:o(f,{title:"Two-factor Authentication (2FA)",children:[t.data.is2FaEnabled&&o("div",{className:"bg-white dark:bg-gray-800 shadow overflow-hidden sm:rounded-lg",children:[a("div",{className:"px-4 py-5 sm:px-6",children:a("div",{className:"flex justify-between",children:a("h3",{className:"text-lg leading-6 font-medium text-gray-900 dark:text-gray-200",children:"Two-Factor Authentication settings"})})}),t.data.recoveryCodesLeft==0?o("div",{className:"flex justify-center items-center",children:[a("p",{children:"You have no recovery codes left."}),a(v,{})]}):t.data.recoveryCodesLeft==1?o("div",{className:"flex justify-center items-center",children:[a("p",{children:"You have one recovery code left."}),a(v,{})]}):t.data.recoveryCodesLeft<=3?o("div",{className:"flex justify-center items-center",children:[o("p",{children:["You have ",t.data.recoveryCodesLeft," recovery codes left."]}),a(v,{})]}):null,o("div",{className:"border-t border-gray-200 flex flex-wrap flex-column gap-5 pt-5 pl-5 pb-5",children:[t.data.isMachineRemembered&&a(me,{}),t.data.is2FaEnabled&&o(h,{children:[a(de,{}),a(v,{})]})]})]}),o("div",{className:"bg-white dark:bg-gray-800 shadow overflow-hidden sm:rounded-lg mt-5",children:[a("div",{className:"px-4 py-5 sm:px-6",children:a("div",{className:"flex justify-between",children:a("h3",{className:"text-lg leading-6 font-medium text-gray-900 dark:text-gray-200",children:"Authenticator app"})})}),o("div",{className:"border-t border-gray-200 flex flex-wrap flex-column gap-5 pt-5 pl-5 pb-5",children:[t.data.hasAuthenticator&&a(ye,{}),a(oe,{options:t.data.validProviders.map(s=>({label:s,value:s}))})]})]})]})},fe=async({data:e})=>c.put("/personal/change-password",e),be=({config:e}={})=>{const{logout:t}=w();return p({onSuccess:async s=>{i.success("Password Updated"),i.success(s.message),await t()},onError:s=>{var r;i.error("Failed to update password"),i.error((r=s.response)==null?void 0:r.data)},...e,mutationFn:fe})},ve=x({password:m(),newPassword:m().min(1,"Required"),confirmNewPassword:m().min(1,"Required")}).refine(e=>e.confirmNewPassword===e.newPassword,{message:"Passwords don't match",path:["confirmNewPassword"]}).refine(e=>e.newPassword!==e.password,{message:"New Password can't be the same as the old one",path:["newPassword"]}),xe=({onSuccess:e})=>{const{user:t}=w(),s=be();return o("div",{className:"bg-white dark:bg-gray-800 shadow overflow-hidden sm:rounded-lg",children:[a("div",{className:"px-4 py-5 sm:px-6",children:a("div",{className:"flex justify-between",children:a("h3",{className:"text-lg leading-6 font-medium text-gray-900 dark:text-gray-200",children:"Change password"})})}),a("div",{className:"bg-white dark:bg-gray-800 shadow overflow-hidden sm:rounded-lg",children:a("div",{className:"flex flex-column flex-wrap gap-5 pl-5 pb-5",children:a(P,{id:"change-password",onSubmit:async r=>{r.userId=t==null?void 0:t.id,await s.mutateAsync({data:r}),e()},schema:ve,children:({register:r,formState:n})=>o(h,{children:[a(u,{label:"Current Password",type:"password",error:n.errors.password,registration:r("password")}),a(u,{label:"New Password",type:"password",error:n.errors.newPassword,registration:r("newPassword")}),a(u,{label:"Confirm New Password",type:"password",error:n.errors.confirmNewPassword,registration:r("confirmNewPassword")}),a("div",{className:"mt-4",children:a(N,{icon:"warning",title:"Change Password",body:"Are you sure you want to update your password? This will log you out.",triggerButton:a(l,{size:"sm",isLoading:s.isLoading,children:"Change Password"}),confirmButton:a(l,{form:"change-password",type:"submit",className:"mt-2",variant:"warning",size:"sm",isLoading:s.isLoading,children:"Submit"})})})]})})})})]})},Ne=async({data:e})=>c.put("/personal/set-password",e),Pe=({config:e}={})=>{const{refetchUser:t}=w();return p({onSuccess:async s=>{i.success("Password set"),i.success(s.message),await t()},onError:s=>{var r;i.error("Failed to set password"),i.error((r=s.response)==null?void 0:r.data)},...e,mutationFn:Ne})},Ae=x({password:m(),confirmPassword:m().min(1,"Required")}).refine(e=>e.confirmPassword===e.password,{message:"Passwords don't match",path:["confirmNewPassword"]}),Ce=({onSuccess:e})=>{const t=Pe();return o("div",{className:"bg-white dark:bg-gray-800 shadow overflow-hidden sm:rounded-lg",children:[a("div",{className:"px-4 py-5 sm:px-6",children:a("div",{className:"flex justify-between",children:a("h3",{className:"text-lg leading-6 font-medium text-gray-900 dark:text-gray-200",children:"Set password"})})}),a("div",{className:"bg-white dark:bg-gray-800 shadow overflow-hidden sm:rounded-lg",children:a("div",{className:"flex flex-column flex-wrap gap-5 pl-5 pb-5",children:a(P,{onSubmit:async s=>{await t.mutateAsync({data:s}),e()},schema:Ae,children:({register:s,formState:r})=>o(h,{children:[a(u,{label:"Password",type:"password",error:r.errors.password,registration:s("password")}),a(u,{label:"Confirm Password",type:"password",error:r.errors.confirmPassword,registration:s("confirmPassword")}),a("div",{children:a(l,{isLoading:t.isLoading,type:"submit",className:"w-full mt-4",children:"Set Password"})})]})})})})]})},Ee=()=>a(f,{title:"Password Management",children:o("div",{className:"bg-white dark:bg-gray-800 shadow overflow-hidden sm:rounded-lg",children:[a("div",{className:"px-4 py-5 sm:px-6",children:a("div",{className:"flex justify-between",children:a("h3",{className:"text-lg leading-6 font-medium text-gray-900 dark:text-gray-200",children:"Manage password"})})}),o("div",{className:"border-t border-gray-200 px-4 py-5 sm:p-0",children:[a(Ce,{onSuccess:()=>window.location.assign("/app")}),a(xe,{onSuccess:()=>window.location.assign("/app")})]})]})});export{Ee as Password,Le as PersonalData,Fe as Profile,Re as TwoFactorAuthentication};
