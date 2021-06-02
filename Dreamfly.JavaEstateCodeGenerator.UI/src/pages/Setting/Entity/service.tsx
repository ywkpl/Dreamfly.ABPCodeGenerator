import request from '@/utils/request';
import appConsts from '@/utils/AppConsts';

export async function generatorCode(params: any) {
  return request(`${appConsts.apiUrl}/entity/generate`, {
    method: 'POST',
    data: params,
  });
}

export async function save(params: any) {
  return request(`${appConsts.apiUrl}/entity/save`, {
    method: 'POST',
    data: params,
  });
}

export async function update(params: any) {
  return request(`${appConsts.apiUrl}/entity/update`, {
    method: 'POST',
    data: params,
  });
}

export async function deleteItems(params: any) {
  return request(`${appConsts.apiUrl}/entity/deleteItems`, {
    method: 'POST',
    data: params,
  });
}

export async function saveTest(params: any) {
  return request(`${appConsts.apiUrl}/entity/saveTest`, {
    method: 'POST',
    data: params,
  });
}

export async function importEntityFromExcel(params: any) {
  return request(`${appConsts.apiUrl}/entity/importFromExcel`, {
    method: 'POST',
    data: params,
  });
}

export async function importEntityFromDb(params: any) {
  return request(`${appConsts.apiUrl}/entity/importFromDB`, {
    method: 'GET',
    params: params,
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
