import request from '@/utils/request';
import appConsts from '@/utils/AppConsts';

export async function generatorCode(params: any) {
  return request(`${appConsts.apiUrl}/entity/generate`, {
    method: 'POST',
    data: params,
  });
}

export async function importEntity(params: any) {
  return request(`${appConsts.apiUrl}/entity/import`, {
    method: 'POST',
    data: params,
  });
}

export async function getEntity(entityName: string) {
  return request(`${appConsts.apiUrl}/entity/get`, {
    method: 'GET',
    params: { entityName: entityName },
  });
}

export async function removeCode(params: any) {
  return request(`${appConsts.apiUrl}/entity/remove`, {
    method: 'POST',
    data: params,
  });
}
